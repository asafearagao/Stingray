@echo off
setlocal
set "PASTA=%~dp0"
powershell.exe -NoProfile -ExecutionPolicy Bypass -Command "Start-Process powershell.exe -Verb RunAs -ArgumentList '-NoProfile -ExecutionPolicy Bypass -File ""%PASTA%Instalar-Stingray.ps1""'"
