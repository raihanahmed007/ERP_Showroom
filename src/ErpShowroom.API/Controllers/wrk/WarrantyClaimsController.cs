using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.wrk.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.wrk;

[Route("api/[controller]")]
public class WarrantyClaimsController : CrudControllerBase<WarrantyClaim>
{
    public WarrantyClaimsController(IUnitOfWork uow) : base(uow) { }
}
