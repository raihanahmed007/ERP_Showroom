using System;
using BCrypt.Net;

namespace ErpShowroom.API.scratch;

public class Program
{
    public static void Main()
    {
        string password = "admin123";
        string hash = BCrypt.Net.BCrypt.HashPassword(password, 12);
        Console.WriteLine(hash);
    }
}
