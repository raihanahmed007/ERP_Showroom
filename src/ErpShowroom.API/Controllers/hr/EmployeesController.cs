using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using ErpShowroom.Application.hr.Commands;
using ErpShowroom.Domain.Common;
using ErpShowroom.Domain.hr.Entities;

namespace ErpShowroom.API.Controllers.hr;

[Route("api/[controller]")]
public class EmployeesController : CrudControllerBase<Employee>
{
    private readonly IMediator _mediator;
    public EmployeesController(IMediator mediator, IUnitOfWork uow) : base(uow)
    {
        _mediator = mediator;
    }

    [HttpPost("attendance")]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("leave")]
    public async Task<IActionResult> ApplyLeave([FromBody] ApplyLeaveCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("payroll/process")]
    [Authorize(Roles = "HRManager")]
    public async Task<IActionResult> ProcessPayroll([FromBody] ProcessPayrollCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { SlipsGenerated = result });
    }
}
