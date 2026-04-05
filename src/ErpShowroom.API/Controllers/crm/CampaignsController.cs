using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.crm;

[Route("api/[controller]")]
public class CampaignsController : CrudControllerBase<Campaign>
{
    public CampaignsController(IUnitOfWork uow) : base(uow) { }
}
