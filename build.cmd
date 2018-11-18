@echo Off

set config=%1
if "%config%" == "" (
    set config=Release
)

cd %~dp0

rem build library and run unit tests
call dotnet restore
if not "%errorlevel%"=="0" goto failure
call dotnet build --configuration %config% --no-restore
if not "%errorlevel%"=="0" goto failure
call dotnet test mvp.xml.tests\mvp.xml.tests.csproj --configuration %config% --no-restore --no-build
if not "%errorlevel%"=="0" goto failure

rem create nuget package in artifacts folder
call dotnet pack mvp.xml\mvp.xml.csproj --configuration %config% --version %GitVersion.NuGetVersion% --output ..\..\artifacts --no-build --no-restore
if not "%errorlevel%"=="0" goto failure

:success
exit /b 0

:failure
exit /b -1
