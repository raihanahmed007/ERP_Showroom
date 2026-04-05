using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.bank.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.bank;

[Route("api/[controller]")]
public class CashDepositsController : CrudControllerBase<CashDeposit>
{
    public CashDepositsController(IUnitOfWork uow) : base(uow) { }
}
