using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prl.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prl;

[Route("api/[controller]")]
public class SalarySlipsController : CrudControllerBase<SalarySlip>
{
    public SalarySlipsController(IUnitOfWork uow) : base(uow) { }
}
