using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.fin.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.fin.Entities;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class AgreementsController : CrudControllerBase<HPAgreement>
{
    private readonly IMediator _mediator;
    public AgreementsController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("hp-agreement")]
    [Authorize(Roles = "FinanceManager")]
    public async Task<IActionResult> CreateHP([FromBody] CreateHPAgreementCommand command)
    {
        var result = await _mediator.Send(command);
        Log.Information("HP Agreement created with reference {AgreementNo}. Value: {Value}", result, command.AgreementValue);
        return Ok(result);
    }

    [HttpGet("overdue")]
    [Authorize(Policy = "RequirePermission:ViewOverdue")]
    public async Task<IActionResult> GetOverdue([FromQuery] RiskBucketEnum riskBucket)
    {
        var result = await _mediator.Send(new GetOverdueAgreementsQuery(riskBucket));
        return Ok(result);
    }

    [HttpPost("{id}/payments")]
    public async Task<IActionResult> AddPayment(long id, [FromBody] AddPaymentCommand command)
    {
        if (id != command.HPAgreementId) return BadRequest();
        var result = await _mediator.Send(command);
        Log.Information("Payment of {Amount} added to HP Agreement {HPAgreementId}", command.Amount, id);
        return Ok(result);
    }
}
