using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class LegalNoticesController : CrudControllerBase<LegalNotice>
{
    public LegalNoticesController(IUnitOfWork uow) : base(uow) { }
}
