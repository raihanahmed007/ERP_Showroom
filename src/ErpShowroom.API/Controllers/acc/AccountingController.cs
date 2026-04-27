using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System;
using System.Threading.Tasks;
using ErpShowroom.Application.acc.Commands;
using ErpShowroom.Application.acc.Queries;
using ErpShowroom.Application.acc.DTOs;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.acc.Entities;

namespace ErpShowroom.API.Controllers.acc;

[Route("api/[controller]")]
[Authorize]
public class AccountingController : CrudControllerBase<JournalEntry>
{
    private readonly IMediator _mediator;
    public AccountingController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetAccounts()
    {
        var result = await _mediator.Send(new GetChartOfAccountsQuery());
        return Ok(result);
    }

    [HttpPost("accounts")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("accounts/{id}")]
    public async Task<IActionResult> UpdateAccount(long id, [FromBody] UpdateAccountCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        if (!result) return BadRequest("Could not update account.");
        return Ok(result);
    }

    [HttpDelete("accounts/{id}")]
    public async Task<IActionResult> DeleteAccount(long id)
    {
        var result = await _mediator.Send(new DeleteAccountCommand(id));
        if (!result) return BadRequest("Could not delete account. It may have existing transactions or was not found.");
        return Ok(result);
    }

    [HttpPost("journal-entries")]
    [Authorize(Policy = "RequirePermission:PostJournal")]
    public async Task<IActionResult> PostJournal([FromBody] CreateJournalEntryCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveVoucher([FromQuery] long journalId)
    {
        // For now using a system user ID or getting from claims
        long userId = 1; // Placeholder, should get from User.FindFirst(ClaimTypes.NameIdentifier)
        var result = await _mediator.Send(new ApproveVoucherCommand(journalId, userId));
        return Ok(result);
    }

    [HttpGet("trial-balance")]
    public async Task<IActionResult> GetTrialBalance([FromQuery] DateTime date)
    {
        var result = await _mediator.Send(new GetTrialBalanceQuery(date));
        return Ok(result);
    }

    [HttpGet("ledger")]
    public async Task<IActionResult> GetLedger([FromQuery] long accountId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _mediator.Send(new GetGeneralLedgerQuery(accountId, startDate, endDate));
        return Ok(result);
    }

    [HttpGet("profit-loss")]
    public async Task<IActionResult> GetProfitLoss([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _mediator.Send(new GetProfitAndLossQuery(startDate, endDate));
        return Ok(result);
    }

    [HttpGet("balance-sheet")]
    public async Task<IActionResult> GetBalanceSheet([FromQuery] DateTime date)
    {
        var result = await _mediator.Send(new GetBalanceSheetQuery(date));
        return Ok(result);
    }

    [HttpPost("close-period")]
    public async Task<IActionResult> ClosePeriod([FromQuery] long periodId)
    {
        var result = await _mediator.Send(new CloseFiscalPeriodCommand(periodId));
        if (!result) return BadRequest("Could not close fiscal period.");
        return Ok(result);
    }
}
