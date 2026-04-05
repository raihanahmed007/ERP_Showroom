using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.crm;

[Route("api/[controller]")]
public class LeadsController : CrudControllerBase<Lead>
{
    public LeadsController(IUnitOfWork uow) : base(uow) { }
}
