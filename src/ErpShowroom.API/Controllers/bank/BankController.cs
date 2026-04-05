using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.bank.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.bank.Entities;

namespace ErpShowroom.API.Controllers.bank;

[Route("api/[controller]")]
[Authorize(Roles = "FinanceManager")]
public class BankController : CrudControllerBase<BankAccount>
{
    private readonly IMediator _mediator;
    public BankController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("reconcile")]
    public async Task<IActionResult> Reconcile([FromBody] ReconcileBankStatementCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }
}
