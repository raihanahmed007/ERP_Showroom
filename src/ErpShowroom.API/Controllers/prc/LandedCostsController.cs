using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prc;

[Route("api/[controller]")]
public class LandedCostsController : CrudControllerBase<LandedCost>
{
    public LandedCostsController(IUnitOfWork uow) : base(uow) { }
}
