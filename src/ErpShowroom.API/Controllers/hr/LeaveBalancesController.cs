using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.hr.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.hr;

[Route("api/[controller]")]
public class LeaveBalancesController : CrudControllerBase<LeaveBalance>
{
    public LeaveBalancesController(IUnitOfWork uow) : base(uow) { }
}
