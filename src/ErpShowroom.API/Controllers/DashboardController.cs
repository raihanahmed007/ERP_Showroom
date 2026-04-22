using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ErpShowroom.API.Controllers;

[ApiController]
[Route("dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    [HttpGet("kpis")]
    public IActionResult GetKpis()
    {
        // Return sample KPI data for the dashboard
        return Ok(new
        {
            CollectionEfficiency = 0.872m,
            PAR = 0.045m,
            RecoveryRate = 0.91m,
            InventoryTurnover = 4.2m
        });
    }
}
