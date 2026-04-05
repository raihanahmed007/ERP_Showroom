using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.acc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.acc;

[Route("api/[controller]")]
public class FiscalPeriodsController : CrudControllerBase<FiscalPeriod>
{
    public FiscalPeriodsController(IUnitOfWork uow) : base(uow) { }
}
