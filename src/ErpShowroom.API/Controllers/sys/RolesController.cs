using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.sys;

[Route("api/[controller]")]
public class RolesController : CrudControllerBase<Role>
{
    public RolesController(IUnitOfWork uow) : base(uow) { }
}
