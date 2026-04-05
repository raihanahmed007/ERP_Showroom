using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.crm.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.crm;

[Route("api/[controller]")]
public class AISentimentLogsController : CrudControllerBase<AISentimentLog>
{
    public AISentimentLogsController(IUnitOfWork uow) : base(uow) { }
}
