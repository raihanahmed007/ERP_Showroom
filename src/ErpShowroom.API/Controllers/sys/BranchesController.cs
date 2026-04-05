using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.sys.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.sys;

[Route("api/[controller]")]
public class BranchesController : CrudControllerBase<Branch>
{
    public BranchesController(IUnitOfWork uow) : base(uow) { }
}
