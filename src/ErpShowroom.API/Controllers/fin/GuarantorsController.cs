using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class GuarantorsController : CrudControllerBase<Guarantor>
{
    public GuarantorsController(IUnitOfWork uow) : base(uow) { }
}
