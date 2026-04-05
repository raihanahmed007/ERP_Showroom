using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wf.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wf;

[Route("api/[controller]")]
public class ApprovalHistoriesController : CrudControllerBase<ApprovalHistory>
{
    public ApprovalHistoriesController(IUnitOfWork uow) : base(uow) { }
}
