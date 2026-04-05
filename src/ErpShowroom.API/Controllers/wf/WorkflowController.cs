using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Application.fin.Workflows;
using Microsoft.AspNetCore.Authorization;

namespace ErpShowroom.API.Controllers.wf;

[ApiController]
[Route("api/workflow")]
[Authorize]
public class WorkflowController : ControllerBase
{
    private readonly WorkflowOrchestrator _orchestrator;

    public WorkflowController(WorkflowOrchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }

    [HttpPost("start-from-lead/{leadId}")]
    public async Task<IActionResult> StartFromLead(long leadId)
    {
        await _orchestrator.StartFromLeadAsync(leadId);
        return Ok(new { message = "Workflow triggered successfully for lead " + leadId });
    }

    [HttpPost("manual-activate/{agreementId}")]
    public async Task<IActionResult> ManualActivate(long agreementId)
    {
        await _orchestrator.ActivateAgreementAsync(agreementId);
        return Ok(new { message = "Activation triggered for agreement " + agreementId });
    }
}
