using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wrk;

[Route("api/[controller]")]
public class JobServicesController : CrudControllerBase<JobService>
{
    public JobServicesController(IUnitOfWork uow) : base(uow) { }
}
