using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prc;

[Route("api/[controller]")]
public class GRNsController : CrudControllerBase<GRN>
{
    public GRNsController(IUnitOfWork uow) : base(uow) { }
}
