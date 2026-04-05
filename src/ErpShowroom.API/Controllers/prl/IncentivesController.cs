using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prl.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prl;

[Route("api/[controller]")]
public class IncentivesController : CrudControllerBase<Incentive>
{
    public IncentivesController(IUnitOfWork uow) : base(uow) { }
}
