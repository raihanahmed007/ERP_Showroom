@echo off
set LOGFILE=C:\Scripts\erp_lifecycle.log
echo [%date% %time%] Stopping ERP System... >> %LOGFILE%

:: 1. Stop Docker Containers
echo Stopping Docker containers (redis, rabbitmq, jaeger)...
docker stop redis rabbitmq jaeger >> %LOGFILE% 2>&1

:: 2. Stop ERP API Windows Service
echo Stopping ERP_API_Service...
sc stop ERP_API_Service >> %LOGFILE% 2>&1

:: 3. Stop IIS App Pool (requires elevated permissions)
echo Stopping IIS App Pool ErpApiAppPool...
C:\Windows\System32\inetsrv\appcmd stop apppool /apppool.name:ErpApiAppPool >> %LOGFILE% 2>&1

echo [%date% %time%] ERP System Stopped. >> %LOGFILE%
echo ---------------------------------------- >> %LOGFILE%
