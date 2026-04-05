@echo off
set LOGFILE=C:\Scripts\erp_lifecycle.log
echo [%date% %time%] Starting ERP System... >> %LOGFILE%

:: 1. Start Docker Containers
echo Starting Docker containers (redis, rabbitmq, jaeger)...
docker start redis rabbitmq jaeger >> %LOGFILE% 2>&1

:: 2. Start ERP API Windows Service
echo Starting ERP_API_Service...
sc start ERP_API_Service >> %LOGFILE% 2>&1

:: 3. Start IIS App Pool (requires elevated permissions)
echo Starting IIS App Pool ErpApiAppPool...
C:\Windows\System32\inetsrv\appcmd start apppool /apppool.name:ErpApiAppPool >> %LOGFILE% 2>&1

echo [%date% %time%] ERP System Started. >> %LOGFILE%
echo ---------------------------------------- >> %LOGFILE%
