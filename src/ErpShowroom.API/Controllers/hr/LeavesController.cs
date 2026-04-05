using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.hr.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.hr;

[Route("api/[controller]")]
public class LeavesController : CrudControllerBase<Leave>
{
    public LeavesController(IUnitOfWork uow) : base(uow) { }
}
