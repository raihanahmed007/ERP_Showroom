$contextFile = "D:\ERP_SHOWROOM\src\ErpShowroom.Infrastructure\Persistence\AppDbContext.cs"
$lines = Get-Content $contextFile
$currentModule = ""

$existingControllers = @(
    "AgreementsController.cs",
    "ProductsController.cs",
    "AccountingController.cs",
    "CustomersController.cs",
    "EmployeesController.cs",
    "JobCardsController.cs",
    "PurchaseOrdersController.cs",
    "DocumentsController.cs",
    "BankController.cs",
    "WorkflowController.cs"
)

foreach ($line in $lines) {
    if ($line -match "^\s*//\s*([a-z]+)\s*$") {
        $currentModule = $matches[1]
    }
    if ($line -match "public DbSet<([a-zA-Z0-9_]+)>\s+([a-zA-Z0-9_]+)\s+=>") {
        $entityName = $matches[1]
        $tableName = $matches[2]
        
        $controllerName = "${tableName}Controller"
        $fileName = "$controllerName.cs"
        
        if ($existingControllers -contains $fileName) {
            continue
        }
        
        $folder = "D:\ERP_SHOWROOM\src\ErpShowroom.API\Controllers\$currentModule"
        if (-not (Test-Path $folder)) {
            New-Item -ItemType Directory -Force -Path $folder | Out-Null
        }
        
        $destFile = "$folder\$fileName"
        if (-not (Test-Path $destFile)) {
            $content = @"
using Microsoft.AspNetCore.Mvc;
using ErpShowroom.Domain.$currentModule.Entities;
using ErpShowroom.Domain.Common;

namespace ErpShowroom.API.Controllers.$currentModule;

[Route("api/[controller]")]
public class $controllerName : CrudControllerBase<$entityName>
{
    public $controllerName(IUnitOfWork uow) : base(uow) { }
}
"@
            Set-Content -Path $destFile -Value $content
            Write-Host "Created $destFile"
        }
    }
}
