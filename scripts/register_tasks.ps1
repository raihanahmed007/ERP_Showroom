# Register Task Scheduler Tasks for ERP System
# Run this script with Administrator permissions to register the lifecycle and maintenance tasks.

Write-Host "Registering ERP Lifecycle Tasks..." -ForegroundColor Cyan

# 1. Shutdown (Daily at 8:00 PM)
schtasks /create /tn "ERP_Shutdown" /tr "C:\Scripts\shutdown_erp.bat" /sc daily /st 20:00 /ru "SYSTEM" /f

# 2. Startup (Daily at 8:00 AM)
schtasks /create /tn "ERP_Startup" /tr "C:\Scripts\startup_erp.bat" /sc daily /st 08:00 /ru "SYSTEM" /f

# 3. Database Backup (Daily at 1:00 AM)
schtasks /create /tn "ERP_Backup" /tr "powershell.exe -ExecutionPolicy Bypass -File C:\Scripts\sql_backup_and_cleanup.ps1" /sc daily /st 01:00 /ru "SYSTEM" /f

# 4. Performance Monitor (Every 15 Minutes)
schtasks /create /tn "ERP_Monitor" /tr "powershell.exe -ExecutionPolicy Bypass -File C:\Scripts\monitor_and_alert.ps1" /sc minute /mo 15 /ru "SYSTEM" /f

Write-Host "All tasks registered successfully!" -ForegroundColor Green
Write-Host "Please ensure you have created the 'C:\Scripts' directory and moved the scripts there." -ForegroundColor Yellow
