using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.inv.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.inv.Entities;

namespace ErpShowroom.API.Controllers.inv;

[Route("api/[controller]")]
public class ProductsController : CrudControllerBase<Product>
{
    private readonly IMediator _mediator;
    public ProductsController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpGet("serial/{serialNo}")]
    public async Task<IActionResult> GetSerialStatus(string serialNo)
    {
        var result = await _mediator.Send(new GetSerialStatusQuery(serialNo));
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost("custom")]
    [Authorize(Roles = "InventoryManager")]
    public async Task<IActionResult> CreateCustom([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("stock/transfer")]
    [Authorize(Policy = "RequirePermission:TransferStock")]
    public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
