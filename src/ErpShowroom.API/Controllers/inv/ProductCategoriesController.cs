using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.inv.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.inv;

[Route("api/[controller]")]
public class ProductCategoriesController : CrudControllerBase<ProductCategory>
{
    public ProductCategoriesController(IUnitOfWork uow) : base(uow) { }
}
