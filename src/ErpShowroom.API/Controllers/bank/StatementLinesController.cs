using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.bank.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.bank;

[Route("api/[controller]")]
public class StatementLinesController : CrudControllerBase<StatementLine>
{
    public StatementLinesController(IUnitOfWork uow) : base(uow) { }
}
