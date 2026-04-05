using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.inv;

[Route("api/[controller]")]
public class StockBalancesController : CrudControllerBase<StockBalance>
{
    public StockBalancesController(IUnitOfWork uow) : base(uow) { }
}
