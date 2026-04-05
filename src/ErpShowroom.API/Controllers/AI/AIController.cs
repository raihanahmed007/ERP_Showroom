using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Application.Common.Interfaces;
using ErpShowroom.Application.AI.DTOs;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace ErpShowroom.API.Controllers.AI;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AIController : ControllerBase
{
    private readonly ISentimentService _sentimentService;
    private readonly IDraftService _draftService;
    private readonly ICreditScoreService _creditScoreService;

    public AIController(
        ISentimentService sentimentService,
        IDraftService draftService,
        ICreditScoreService creditScoreService)
    {
        _sentimentService = sentimentService;
        _draftService = draftService;
        _creditScoreService = creditScoreService;
    }

    [HttpPost("sentiment")]
    public async Task<ActionResult<SentimentResult>> AnalyzeSentiment([FromBody] SentimentRequest request, CancellationToken ct)
    {
        var result = await _sentimentService.AnalyzeSentimentAsync(request.CustomerId, request.ConversationText, ct);
        return Ok(result);
    }

    [HttpPost("draft-reminder")]
    public async Task<ActionResult<DraftReminderResponse>> DraftReminder([FromBody] DraftReminderRequest request, CancellationToken ct)
    {
        var draft = await _draftService.DraftReminderAsync(request.OverdueDays, request.CustomerName, request.OutstandingAmount, ct);
        return Ok(new DraftReminderResponse { DraftMessage = draft });
    }

    [HttpPost("credit-score")]
    public async Task<ActionResult<CreditScoreResponse>> CalculateCreditScore([FromBody] CreditScoreRequest request, CancellationToken ct)
    {
        var score = await _creditScoreService.CalculateCreditScoreAsync(request.CustomerId, ct);
        return Ok(new CreditScoreResponse { CreditScore = score });
    }
}
