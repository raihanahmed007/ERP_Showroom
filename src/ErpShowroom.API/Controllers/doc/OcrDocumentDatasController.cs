using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.doc.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.doc;

[Route("api/[controller]")]
public class OcrDocumentDatasController : CrudControllerBase<OcrDocumentData>
{
    public OcrDocumentDatasController(IUnitOfWork uow) : base(uow) { }
}
