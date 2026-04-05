using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class RepossessionCasesController : CrudControllerBase<RepossessionCase>
{
    public RepossessionCasesController(IUnitOfWork uow) : base(uow) { }
}
