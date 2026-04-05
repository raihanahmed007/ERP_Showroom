using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.prl.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.prl;

[Route("api/[controller]")]
public class SalaryStructuresController : CrudControllerBase<SalaryStructure>
{
    public SalaryStructuresController(IUnitOfWork uow) : base(uow) { }
}
