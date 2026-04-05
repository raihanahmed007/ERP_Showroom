param(
    [string]$SourcePath = "./api-publish",
    [string]$DestPath = "D:\ErpShowroom\api",
    [string]$DbConnString = "Server=localhost\SQLEXPRESS;Database=ErpDB;Trusted_Connection=True;TrustServerCertificate=True;",
    [string]$AppPoolName = "ErpApiAppPool",
    [string]$WebsiteName = "ErpApi"
)

Write-Host "--- ERP API Deployment Script ---" -ForegroundColor Cyan

# 1. Ensure Destination exists
if (!(Test-Path $DestPath)) {
    New-Item -ItemType Directory -Path $DestPath -Force
}

# 2. Stop IIS Website and App Pool
Write-Host "Stopping IIS Website and App Pool..."
Stop-WebSite -Name $WebsiteName -ErrorAction SilentlyContinue
Stop-WebAppPool -Name $AppPoolName -ErrorAction SilentlyContinue

# 3. Copy Files
Write-Host "Copying files from $SourcePath to $DestPath..."
Copy-Item "$SourcePath\*" $DestPath -Recurse -Force

# 4. Update appsettings.json
$configPath = "$DestPath\appsettings.json"
if (Test-Path $configPath) {
    Write-Host "Updating connection string in appsettings.json..."
    $config = Get-Content $configPath | ConvertFrom-Json
    $config.ConnectionStrings.DefaultConnection = $DbConnString
    $config | ConvertTo-Json -Depth 20 | Set-Content $configPath
}

# 5. Start App Pool and Website
Write-Host "Starting IIS Website and App Pool..."
Start-WebAppPool -Name $AppPoolName
Start-WebSite -Name $WebsiteName

# 6. Run database migrations (Using dotnet ef or generic migration logic in app)
Write-Host "Database migrations will run on first API startup (EnsureCreated/Migrate logic)."

Write-Host "Deployment Completed Successfully!" -ForegroundColor Green
