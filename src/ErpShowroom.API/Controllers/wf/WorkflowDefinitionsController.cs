using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wf.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wf;

[Route("api/[controller]")]
public class WorkflowDefinitionsController : CrudControllerBase<WorkflowDefinition>
{
    public WorkflowDefinitionsController(IUnitOfWork uow) : base(uow) { }
}
