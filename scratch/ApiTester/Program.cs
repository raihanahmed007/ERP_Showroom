using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace ApiTester
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = "http://localhost:5230";
            var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

            Console.WriteLine("=====================================");
            Console.WriteLine(" ERP ACCOUNTING MODULE AUTO-TESTER ");
            Console.WriteLine("=====================================");

            // 1. Authentication
            Console.WriteLine("\n[1] Testing Authentication...");
            var loginRequest = new { UsernamePhoneOrEmail = "admin@erpshowroom.com", Password = "Admin@123" };
            var loginResponse = await client.PostAsJsonAsync("/auth/login", loginRequest);
            
            if (!loginResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"[FAILED] Login failed: {loginResponse.StatusCode}");
                return;
            }
            
            var tokenString = await loginResponse.Content.ReadAsStringAsync();
            var tokenElement = JsonSerializer.Deserialize<JsonElement>(tokenString);
            var token = tokenElement.GetProperty("accessToken").GetString();
            Console.WriteLine($"[SUCCESS] Logged in successfully. Token length: {token?.Length}");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // 2. Chart of Accounts Fetch
            Console.WriteLine("\n[2] Fetching Chart of Accounts...");
            var coaResponse = await client.GetAsync("/api/accounting/accounts");
            if (!coaResponse.IsSuccessStatusCode)
            {
                var err = await coaResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"[FAILED] Failed to fetch Chart of Accounts: {coaResponse.StatusCode} - {err}");
                return;
            }
            var coaList = await coaResponse.Content.ReadFromJsonAsync<List<JsonElement>>();
            Console.WriteLine($"[SUCCESS] Fetched {coaList?.Count} accounts from Chart of Accounts.");

            long? cashAccountId = null;
            long? equityAccountId = null;
            foreach(var acc in coaList) {
                var props = acc.EnumerateObject().ToDictionary(p => p.Name.ToLower(), p => p.Value);
                if(props.ContainsKey("accountname") && props["accountname"].GetString()!.Contains("Cash")) {
                    cashAccountId = props["id"].GetInt64();
                }
                if(props.ContainsKey("accountname") && (props["accountname"].GetString()!.Contains("Equity") || props["accountname"].GetString()!.Contains("Capital"))) {
                    equityAccountId = props["id"].GetInt64();
                }
                // Enum types not checked to avoid json parse errors
            }
            if (cashAccountId == null && coaList?.Count > 0) cashAccountId = coaList.First().GetProperty("id").GetInt64();
            if (equityAccountId == null && coaList?.Count > 1) equityAccountId = coaList.Last().GetProperty("id").GetInt64();

            // 3. Post a Voucher (Journal Entry)
            if (cashAccountId.HasValue && equityAccountId.HasValue)
            {
                Console.WriteLine($"\n[3] Posting a Journal Entry (Voucher)...");
                Console.WriteLine($"    Debit Account: {cashAccountId.Value}, Credit Account: {equityAccountId.Value}");
                
                var journalEntry = new
                {
                    JournalDate = DateTime.UtcNow,
                    ReferenceNumber = "TEST-VOUCHER-" + DateTime.Now.Ticks,
                    Description = "Automated test voucher",
                    TotalDebit = 1000m,
                    TotalCredit = 1000m,
                    Lines = new[]
                    {
                        new { AccountId = cashAccountId.Value, DebitAmount = 1000m, CreditAmount = 0m, Description = "Test Debit" },
                        new { AccountId = equityAccountId.Value, DebitAmount = 0m, CreditAmount = 1000m, Description = "Test Credit" }
                    }
                };

                var jeResponse = await client.PostAsJsonAsync("/api/accounting/journal-entries", journalEntry);
                if (jeResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("[SUCCESS] Voucher posted successfully.");
                }
                else
                {
                    var error = await jeResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"[FAILED] Voucher posting failed: {jeResponse.StatusCode} - {(error.Length > 200 ? error.Substring(0, 200) + "..." : error)}");
                }
            }

            // 4. Fetch Trial Balance
            Console.WriteLine("\n[4] Fetching Trial Balance...");
            var tbResponse = await client.GetAsync($"/api/accounting/trial-balance?date={DateTime.UtcNow:yyyy-MM-dd}");
            if (tbResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("[SUCCESS] Trial Balance fetched successfully.");
            }
            else
            {
                 var tbError = await tbResponse.Content.ReadAsStringAsync();
                 Console.WriteLine($"[FAILED] Could not fetch Trial Balance: {tbResponse.StatusCode} - {(tbError.Length > 200 ? tbError.Substring(0, 200) + "..." : tbError)}");
            }

            Console.WriteLine("\n=====================================");
            Console.WriteLine("         TEST SUITE COMPLETE           ");
            Console.WriteLine("=====================================");
        }
    }
}
