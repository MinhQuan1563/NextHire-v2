@echo off
REM NextHire-v2 Quick Setup Script
REM This script helps you set up the NextHire-v2 project quickly

echo 🚀 NextHire-v2 Quick Setup Script
echo ==================================

REM Check if .NET is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ❌ .NET SDK is not installed. Please install .NET 9.0 SDK first.
    echo Download from: https://dotnet.microsoft.com/download/dotnet/9.0
    pause
    exit /b 1
)

REM Check .NET version
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET SDK version: %DOTNET_VERSION%

REM Check if version starts with 9
echo %DOTNET_VERSION% | findstr /B "9" >nul
if errorlevel 1 (
    echo ⚠️  Warning: This project requires .NET 9.0 SDK
    echo Your current version: %DOTNET_VERSION%
    echo Download .NET 9.0 from: https://dotnet.microsoft.com/download/dotnet/9.0
    
    set /p CONTINUE="Do you want to continue anyway? (y/N): "
    if /i not "%CONTINUE%"=="y" exit /b 1
)

REM Navigate to project directory
set PROJECT_DIR=src\NextHireApp.HttpApi.Host
if not exist "%PROJECT_DIR%" (
    echo ❌ Project directory not found: %PROJECT_DIR%
    echo Make sure you're running this script from the repository root.
    pause
    exit /b 1
)

echo 📁 Navigating to project directory: %PROJECT_DIR%
cd "%PROJECT_DIR%"

REM Install ABP CLI if not already installed
echo 🔧 Checking ABP CLI...
abp --version >nul 2>&1
if errorlevel 1 (
    echo 📦 Installing ABP CLI...
    dotnet tool install -g Volo.Abp.Cli
) else (
    echo ✅ ABP CLI is already installed
)

REM Restore NuGet packages
echo 📦 Restoring NuGet packages...
dotnet restore

if errorlevel 1 (
    echo ❌ Failed to restore NuGet packages
    echo This might be due to .NET version mismatch.
    echo Please check the troubleshooting section in TUTORIAL.md
    pause
    exit /b 1
)

REM Install client-side libraries
echo 📦 Installing client-side libraries...
abp install-libs

REM Generate development certificate
echo 🔐 Generating development certificate...
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c
dotnet dev-certs https --trust

echo.
echo ✅ Setup completed successfully!
echo.
echo 📖 Next steps:
echo 1. Configure your database connection string in appsettings.json
echo 2. Run the DbMigrator: cd ..\NextHireApp.DbMigrator ^&^& dotnet run
echo 3. Start the application: dotnet run
echo.
echo 📚 For detailed instructions, see:
echo    - English: TUTORIAL.md
echo    - Vietnamese: TUTORIAL-VI.md
echo    - Video: https://www.youtube.com/watch?v=ZAJwVLaTnOM

pause