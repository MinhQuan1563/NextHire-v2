#!/bin/bash

# NextHire-v2 Quick Setup Script
# This script helps you set up the NextHire-v2 project quickly

echo "🚀 NextHire-v2 Quick Setup Script"
echo "=================================="

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 9.0 SDK first."
    echo "Download from: https://dotnet.microsoft.com/download/dotnet/9.0"
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET SDK version: $DOTNET_VERSION"

# Check if version starts with 9
if [[ $DOTNET_VERSION != 9.* ]]; then
    echo "⚠️  Warning: This project requires .NET 9.0 SDK"
    echo "Your current version: $DOTNET_VERSION"
    echo "Download .NET 9.0 from: https://dotnet.microsoft.com/download/dotnet/9.0"
    
    read -p "Do you want to continue anyway? (y/N): " -n 1 -r
    echo
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

# Navigate to project directory
PROJECT_DIR="src/NextHireApp.HttpApi.Host"
if [ ! -d "$PROJECT_DIR" ]; then
    echo "❌ Project directory not found: $PROJECT_DIR"
    echo "Make sure you're running this script from the repository root."
    exit 1
fi

echo "📁 Navigating to project directory: $PROJECT_DIR"
cd "$PROJECT_DIR"

# Install ABP CLI if not already installed
echo "🔧 Checking ABP CLI..."
if ! command -v abp &> /dev/null; then
    echo "📦 Installing ABP CLI..."
    dotnet tool install -g Volo.Abp.Cli
else
    echo "✅ ABP CLI is already installed"
fi

# Restore NuGet packages
echo "📦 Restoring NuGet packages..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ Failed to restore NuGet packages"
    echo "This might be due to .NET version mismatch."
    echo "Please check the troubleshooting section in TUTORIAL.md"
    exit 1
fi

# Install client-side libraries
echo "📦 Installing client-side libraries..."
abp install-libs

# Generate development certificate
echo "🔐 Generating development certificate..."
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c
dotnet dev-certs https --trust

echo ""
echo "✅ Setup completed successfully!"
echo ""
echo "📖 Next steps:"
echo "1. Configure your database connection string in appsettings.json"
echo "2. Run the DbMigrator: cd ../NextHireApp.DbMigrator && dotnet run"
echo "3. Start the application: dotnet run"
echo ""
echo "📚 For detailed instructions, see:"
echo "   - English: TUTORIAL.md"
echo "   - Vietnamese: TUTORIAL-VI.md"
echo "   - Video: https://www.youtube.com/watch?v=ZAJwVLaTnOM"