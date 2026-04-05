# Performance Monitoring & Alerting
$CpuThreshold = 90
$RamThreshold = 85
$DiskSpaceThreshold = 10  # GB
$Recipient = "admin@yourcompany.com"
$SmtpServer = "smtp.yourcompany.com"
$LogPath = "C:\Scripts\perf_log.csv"

# 1. Collect Stats
$CpuUsage = (Get-Counter '\Processor(_Total)\% Processor Time' -ErrorAction SilentlyContinue).CounterSamples.CookedValue
$Ram = Get-CimInstance -ClassName Win32_OperatingSystem
$RamUsage = (($Ram.TotalVisibleMemorySize - $Ram.FreePhysicalMemory) / $Ram.TotalVisibleMemorySize) * 100
$DiskArr = Get-CimInstance Win32_LogicalDisk | Where-Object { $_.DeviceID -eq 'C:' }
$DiskFreeGB = [math]::Round($DiskArr.FreeSpace / 1GB, 2)

# 2. Log Data (CSV Format)
$LogTimestamp = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
$Entry = "$LogTimestamp,$([math]::Round($CpuUsage, 2)),$([math]::Round($RamUsage, 2)),$DiskFreeGB"
if (!(Test-Path $LogPath)) {
    Add-Content -Path $LogPath -Value "Timestamp,CpuUsage,RamUsage,DiskFreeGB"
}
Add-Content -Path $LogPath -Value $Entry

# 3. Check Thresholds and Alert
$Alerts = @()
if ($CpuUsage -gt $CpuThreshold) { $Alerts += "CPU High: $($CpuUsage)% exceeds $($CpuThreshold)%" }
if ($RamUsage -gt $RamThreshold) { $Alerts += "RAM High: $($RamUsage)% exceeds $($RamThreshold)%" }
if ($DiskFreeGB -lt $DiskSpaceThreshold) { $Alerts += "Disk Space Low: $($DiskFreeGB)GB remains below $($DiskSpaceThreshold)GB" }

# 4. Send Email Alert if any threshold is crossed
if ($Alerts.Count -gt 0) {
    Write-Warning "Performance Threshold Crossed! Sending Alert..."
    $Body = "The following performance alerts have been triggered on ERP server:`n`n" + ($Alerts -join "`n")
    Send-MailMessage -SmtpServer $SmtpServer -From "erp-monitor@localhost" -To $Recipient -Subject "ERP SERVER ALERT - Critical Performance" -Body $Body
}
