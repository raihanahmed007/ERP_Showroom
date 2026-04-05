using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers;

[ApiController]
[Authorize]
public abstract class CrudControllerBase<T> : ControllerBase where T : BaseEntity
{
    protected readonly IUnitOfWork _uow;

    protected CrudControllerBase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetAll()
    {
        var items = await _uow.Repository<T>().GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(long id)
    {
        var item = await _uow.Repository<T>().GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public virtual async Task<IActionResult> Create([FromBody] T entity)
    {
        if (entity == null) return BadRequest();
        await _uow.Repository<T>().AddAsync(entity);
        await _uow.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> Update(long id, [FromBody] T entity)
    {
        if (id != entity.Id) return BadRequest();
        var existing = await _uow.Repository<T>().GetByIdAsync(id);
        if (existing == null) return NotFound();
        
        _uow.Repository<T>().Update(entity);
        await _uow.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> Delete(long id)
    {
        var existing = await _uow.Repository<T>().GetByIdAsync(id);
        if (existing == null) return NotFound();
        
        _uow.Repository<T>().Delete(existing);
        await _uow.SaveChangesAsync();
        return NoContent();
    }
}
