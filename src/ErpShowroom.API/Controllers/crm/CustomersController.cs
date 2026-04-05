using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.crm.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.crm.Entities;

namespace ErpShowroom.API.Controllers.crm;

[Route("api/[controller]")]
public class CustomersController : CrudControllerBase<Customer>
{
    private readonly IMediator _mediator;
    public CustomersController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("custom")]
    public async Task<IActionResult> CreateCustom([FromBody] CreateCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("lead/convert")]
    [Authorize(Policy = "RequirePermission:ManageLeads")]
    public async Task<IActionResult> ConvertLead([FromBody] ConvertLeadToCustomerCommand command)
    {
        var result = await _mediator.Send(command);
        return result == null ? NotFound() : Ok(result);
    }
}
