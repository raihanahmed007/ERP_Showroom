using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.bank.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.bank;

[Route("api/[controller]")]
public class BankStatementsController : CrudControllerBase<BankStatement>
{
    public BankStatementsController(IUnitOfWork uow) : base(uow) { }
}
