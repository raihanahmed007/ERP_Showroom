using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.bank.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.bank;

[Route("api/[controller]")]
public class BankAccountsController : CrudControllerBase<BankAccount>
{
    public BankAccountsController(IUnitOfWork uow) : base(uow) { }
}
