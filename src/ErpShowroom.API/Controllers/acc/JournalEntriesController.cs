using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ErpShowroom.Domain.acc.Entities;
using ErpShowroom.Infrastructure.Persistence;
using ErpShowroom.Application.acc.DTOs;

namespace ErpShowroom.API.Controllers.acc;

[Route("api/[controller]")]
public class JournalEntriesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var entries = await context.JournalEntries
            .OrderByDescending(j => j.JournalDate)
            .Select(j => new JournalEntryDto(
                j.Id,
                j.VoucherNo ?? "",
                j.VoucherType.GetValueOrDefault(),
                j.JournalDate.GetValueOrDefault(),
                j.Narration ?? "",
                j.JournalLines.Sum(l => l.DebitAmount ?? 0),
                j.JournalLines.Sum(l => l.CreditAmount ?? 0),
                j.Status,
                j.JournalLines.Select(l => new JournalLineDto(
                    l.AccountId.GetValueOrDefault(),
                    l.DebitAmount ?? 0,
                    l.CreditAmount ?? 0,
                    l.Description,
                    l.Account!.AccountName
                )).ToList()
            ))
            .ToListAsync();

        return Ok(entries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var entry = await context.JournalEntries
            .Where(j => j.Id == id)
            .Select(j => new JournalEntryDto(
                j.Id,
                j.VoucherNo ?? "",
                j.VoucherType.GetValueOrDefault(),
                j.JournalDate.GetValueOrDefault(),
                j.Narration ?? "",
                j.JournalLines.Sum(l => l.DebitAmount ?? 0),
                j.JournalLines.Sum(l => l.CreditAmount ?? 0),
                j.Status,
                j.JournalLines.Select(l => new JournalLineDto(
                    l.AccountId.GetValueOrDefault(),
                    l.DebitAmount ?? 0,
                    l.CreditAmount ?? 0,
                    l.Description,
                    l.Account!.AccountName
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        if (entry == null) return NotFound();
        return Ok(entry);
    }
}
