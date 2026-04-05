using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.acc.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.acc.Entities;

namespace ErpShowroom.API.Controllers.acc;

[Route("api/[controller]")]
[Authorize(Roles = "FinanceManager")]
public class AccountingController : CrudControllerBase<JournalEntry>
{
    private readonly IMediator _mediator;
    public AccountingController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("journal-entries")]
    [Authorize(Policy = "RequirePermission:PostJournal")]
    public async Task<IActionResult> PostJournal([FromBody] CreateJournalEntryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("trial-balance")]
    public async Task<IActionResult> GetTrialBalance([FromQuery] long periodId)
    {
        var result = await _mediator.Send(new GetTrialBalanceQuery(periodId));
        return Ok(result);
    }
}
