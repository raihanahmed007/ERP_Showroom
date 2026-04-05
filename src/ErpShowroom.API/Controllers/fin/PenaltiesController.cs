using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class PenaltiesController : CrudControllerBase<Penalty>
{
    public PenaltiesController(IUnitOfWork uow) : base(uow) { }
}
