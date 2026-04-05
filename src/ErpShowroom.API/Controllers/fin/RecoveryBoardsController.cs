using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.fin.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.fin;

[Route("api/[controller]")]
public class RecoveryBoardsController : CrudControllerBase<RecoveryBoard>
{
    public RecoveryBoardsController(IUnitOfWork uow) : base(uow) { }
}
