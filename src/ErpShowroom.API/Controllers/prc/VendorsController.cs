using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prc;

[Route("api/[controller]")]
public class VendorsController : CrudControllerBase<Vendor>
{
    public VendorsController(IUnitOfWork uow) : base(uow) { }
}
