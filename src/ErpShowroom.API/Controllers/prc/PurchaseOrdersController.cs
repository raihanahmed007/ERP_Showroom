using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.prc.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.prc.Entities;

namespace ErpShowroom.API.Controllers.prc;

[Route("api/[controller]")]
[Authorize(Roles = "ProcurementManager")]
public class PurchaseOrdersController : CrudControllerBase<PurchaseOrder>
{
    private readonly IMediator _mediator;
    public PurchaseOrdersController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("custom")]
    public async Task<IActionResult> CreateCustom([FromBody] CreatePurchaseOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("grn")]
    public async Task<IActionResult> Receive([FromBody] ReceiveGoodsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
