using System.Threading;
using System.Threading.Tasks;
using ErpShowroom.Application.AI.DTOs;

namespace ErpShowroom.Application.Common.Interfaces;

public interface ISentimentService
{
    Task<SentimentResult> AnalyzeSentimentAsync(long customerId, string conversationText, CancellationToken ct = default);
}

public interface IDraftService
{
    Task<string> DraftReminderAsync(int overdueDays, string customerName, decimal outstandingAmount, CancellationToken ct = default);
}

public interface ICreditScoreService
{
    Task<int> CalculateCreditScoreAsync(long customerId, CancellationToken ct = default);
}
