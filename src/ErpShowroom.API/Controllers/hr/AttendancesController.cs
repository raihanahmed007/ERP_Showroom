using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.hr.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.hr;

[Route("api/[controller]")]
public class AttendancesController : CrudControllerBase<Attendance>
{
    public AttendancesController(IUnitOfWork uow) : base(uow) { }
}
