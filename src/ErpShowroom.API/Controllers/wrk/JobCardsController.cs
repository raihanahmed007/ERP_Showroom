using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.wrk.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.wrk.Entities;

namespace ErpShowroom.API.Controllers.wrk;

[Route("api/[controller]")]
public class JobCardsController : CrudControllerBase<JobCard>
{
    private readonly IMediator _mediator;
    public JobCardsController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("custom")]
    public async Task<IActionResult> CreateCustom([FromBody] CreateJobCardCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}/complete")]
    [Authorize(Roles = "WorkshopManager")]
    public async Task<IActionResult> Complete(long id, [FromBody] CompleteJobCardCommand command)
    {
        if (id != command.JobCardId) return BadRequest();
        var result = await _mediator.Send(command);
        return result ? Ok() : NotFound();
    }
}
