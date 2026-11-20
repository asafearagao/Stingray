@echo off
setlocal
set "PASTA=%~dp0"
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%PASTA%Instalar-Stingray.ps1"
echo.
pause
