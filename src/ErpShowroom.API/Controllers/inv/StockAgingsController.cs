using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.inv;

[Route("api/[controller]")]
public class StockAgingsController : CrudControllerBase<StockAging>
{
    public StockAgingsController(IUnitOfWork uow) : base(uow) { }
}
