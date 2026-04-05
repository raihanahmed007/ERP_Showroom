using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.inv;

[Route("api/[controller]")]
public class BrandsController : CrudControllerBase<Brand>
{
    public BrandsController(IUnitOfWork uow) : base(uow) { }
}
