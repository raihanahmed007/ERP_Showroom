# Variables
$Server = "localhost\SQLEXPRESS"
$Database = "ErpDB"
$BackupFolder = "C:\Backups"
$ArchiveFolder = "S:\Backups\Archive"  # External or long-term drive
$RetentionDays = 14  # Main folder retention
$EmailAlerts = $true
$SmtpServer = "smtp.yourcompany.com"  # Internal relay
$To = "admin@yourcompany.com"

# Create Backup if not exists
if (!(Test-Path $BackupFolder)) {
    New-Item -ItemType Directory -Path $BackupFolder
}

# 1. Database Backup
$Timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$Filename = "ERP_$($Timestamp).bak"
$FullPath = "$BackupFolder\$Filename"

Write-Host "Backing up database [$Database] to $FullPath..." -ForegroundColor Cyan

# SQL Command for Backup
$Query = "BACKUP DATABASE [$Database] TO DISK = '$FullPath' WITH INIT, COMPRESSION"
sqlcmd -S $Server -Q $Query

# Check if successful
if ($LASTEXITCODE -eq 0) {
    Write-Host "Backup Successful: $Filename" -ForegroundColor Green
} else {
    Write-Error "Backup Failed!"
    if ($EmailAlerts) {
        Send-MailMessage -SmtpServer $SmtpServer -From "erp@localhost" -To $To -Subject "ERP Backup FAILED" -Body "An error occurred during database backup at $(Get-Date)."
    }
}

# 2. Storage Cleanup & Archival
# Delete backups older than $RetentionDays locally
Write-Host "Cleaning up old local backups..."
Get-ChildItem -Path $BackupFolder -Filter "*.bak" | Where-Object { $_.LastWriteTime -lt (Get-Date).AddDays(-$RetentionDays) } | Remove-Item -Force

# Optional: Copy new backup to archival storage
if (Test-Path $ArchiveFolder) {
    Write-Host "Archiving backup to persistent storage..."
    Copy-Item $FullPath $ArchiveFolder
}
