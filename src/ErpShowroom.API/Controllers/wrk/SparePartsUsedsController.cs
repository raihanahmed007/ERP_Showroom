using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wrk;

[Route("api/[controller]")]
public class SparePartsUsedsController : CrudControllerBase<SparePartUsed>
{
    public SparePartsUsedsController(IUnitOfWork uow) : base(uow) { }
}
