using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.sys;

[Route("api/[controller]")]
public class UsersController : CrudControllerBase<User>
{
    public UsersController(IUnitOfWork uow) : base(uow) { }
}
