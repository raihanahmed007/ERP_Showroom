using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wf.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wf;

[Route("api/[controller]")]
public class WorkflowStepsController : CrudControllerBase<WorkflowStep>
{
    public WorkflowStepsController(IUnitOfWork uow) : base(uow) { }
}
