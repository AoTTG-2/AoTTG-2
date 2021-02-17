@ECHO OFF

setlocal

rem start with editor install layout
set CSC=%~dp0..\..\Tools\Roslyn\csc.exe

rem fall back to source tree layout
if not exist "%CSC%" set CSC=%~dp0..\..\csc\builds\Binaries\Windows\csc.exe

if not exist "%CSC%" (
	echo Failed to find csc.exe
	exit /b 1
)

"%CSC%" /shared %*
exit /b %ERRORLEVEL%

endlocal
