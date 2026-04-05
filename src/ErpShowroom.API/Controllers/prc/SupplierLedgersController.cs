using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prc;

[Route("api/[controller]")]
public class SupplierLedgersController : CrudControllerBase<SupplierLedger>
{
    public SupplierLedgersController(IUnitOfWork uow) : base(uow) { }
}
