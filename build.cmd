@echo off

if '%1'=='/?' goto help
if '%1'=='-help' goto help
if '%1'=='-h' goto help

powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\build.ps1' %*; if ($psake.build_success -eq $false) { exit 1 } else { exit 0 }"
IF %ERRORLEVEL% == 0 EXIT /B 0
ECHO ##teamcity[buildStatus status='FAILURE' text='The build and run unit step failed']
EXIT /B 1 
goto :eof

:help
powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\build.ps1' Help"
