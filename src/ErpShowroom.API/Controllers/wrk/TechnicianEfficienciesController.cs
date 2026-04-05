using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wrk;

[Route("api/[controller]")]
public class TechnicianEfficienciesController : CrudControllerBase<TechnicianEfficiency>
{
    public TechnicianEfficienciesController(IUnitOfWork uow) : base(uow) { }
}
