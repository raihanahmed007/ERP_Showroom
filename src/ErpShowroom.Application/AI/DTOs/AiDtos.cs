using System;

namespace ErpShowroom.Application.AI.DTOs;

public class SentimentRequest
{
    public long CustomerId { get; set; }
    public string ConversationText { get; set; } = string.Empty;
}

public class SentimentResult
{
    public string Sentiment { get; set; } = "neutral";
    public decimal Score { get; set; } = 0.5m;
    public long? LogId { get; set; }
}

public class DraftReminderRequest
{
    public int OverdueDays { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal OutstandingAmount { get; set; }
}

public class DraftReminderResponse
{
    public string DraftMessage { get; set; } = string.Empty;
}

public class CreditScoreRequest
{
    public long CustomerId { get; set; }
}

public class CreditScoreResponse
{
    public int CreditScore { get; set; }
}
