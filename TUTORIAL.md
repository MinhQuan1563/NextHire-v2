# NextHire-v2 Project Tutorial - Step by Step Guide

## Video Tutorial Reference
**Video**: https://www.youtube.com/watch?v=ZAJwVLaTnOM

This tutorial provides detailed step-by-step instructions for setting up, configuring, and deploying the NextHire-v2 project using ABP Framework with Jenkins CI/CD and Docker containerization.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Project Overview](#project-overview)
3. [Local Development Setup](#local-development-setup)
4. [Database Configuration](#database-configuration)
5. [Jenkins CI/CD Pipeline Setup](#jenkins-cicd-pipeline-setup)
6. [Docker Deployment](#docker-deployment)
7. [Monitoring and Notifications](#monitoring-and-notifications)
8. [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Software
- **.NET 9.0+ SDK** - [Download](https://dotnet.microsoft.com/download/dotnet)
- **Node.js v20.11+** - [Download](https://nodejs.org/en)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **Jenkins** - [Download](https://www.jenkins.io/download/)
- **Git** - [Download](https://git-scm.com/downloads)
- **Visual Studio Code or Visual Studio** - IDE of choice

### Account Requirements
- GitHub account for version control
- Discord server for notifications (optional)
- Email account for Jenkins notifications

## Project Overview

NextHire-v2 is an ABP Framework-based web application with the following architecture:

### Project Structure
```
NextHire-v2/
├── src/
│   ├── NextHireApp.Application/          # Application services
│   ├── NextHireApp.Application.Contracts/ # Application contracts
│   ├── NextHireApp.DbMigrator/           # Database migration tool
│   ├── NextHireApp.Domain/               # Domain entities and services
│   ├── NextHireApp.Domain.Shared/        # Shared domain components
│   ├── NextHireApp.EntityFrameworkCore/  # EF Core configuration
│   ├── NextHireApp.HttpApi/              # HTTP API controllers
│   ├── NextHireApp.HttpApi.Client/       # HTTP API client
│   └── NextHireApp.HttpApi.Host/         # Web API host
├── Jenkinsfile                           # CI/CD pipeline
├── Dockerfile                            # Container configuration
└── README.md                             # Project documentation
```

### Key Technologies
- **ABP Framework**: Modern application framework for .NET
- **Entity Framework Core**: Object-relational mapping
- **OpenIddict**: OAuth 2.0/OpenID Connect server
- **Jenkins**: Continuous integration and deployment
- **Docker**: Containerization platform

## Local Development Setup

### Quick Setup (Recommended)

For a quick automated setup, use the provided setup scripts:

```bash
# Linux/macOS
./setup.sh

# Windows
setup.bat
```

These scripts will automatically:
- Check .NET SDK version
- Install ABP CLI if needed
- Restore NuGet packages
- Install client-side libraries
- Generate SSL certificates

### Manual Setup

If you prefer to set up manually, follow these steps:

### Step 1: Clone the Repository
```bash
git clone https://github.com/MinhQuan1563/NextHire-v2.git
cd NextHire-v2
```

### Step 2: Restore Dependencies
```bash
# Navigate to the main project directory
cd src/NextHireApp.HttpApi.Host

# Restore .NET packages
dotnet restore

# Install ABP CLI (if not already installed)
dotnet tool install -g Volo.Abp.Cli

# Install client-side libraries
abp install-libs
```

### Step 3: Configure Environment Variables
Create a `appsettings.Development.json` file in `src/NextHireApp.HttpApi.Host/`:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=NextHireApp;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Step 4: Generate SSL Certificate
```bash
# Generate development certificate for HTTPS
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c

# Trust the certificate
dotnet dev-certs https --trust
```

## Database Configuration

### Step 1: Configure Connection String
Update the connection string in `appsettings.json` to match your database setup:

```json
{
  "ConnectionStrings": {
    "Default": "Server=your-server;Database=NextHireApp;User Id=your-username;Password=your-password;TrustServerCertificate=true"
  }
}
```

### Step 2: Run Database Migration
```bash
# Navigate to the DbMigrator project
cd src/NextHireApp.DbMigrator

# Run the database migrator
dotnet run
```

This will:
- Create the database if it doesn't exist
- Apply all Entity Framework migrations
- Seed initial data including admin user

### Step 3: Verify Database Setup
- Default admin user: `admin`
- Default admin password: `1q2w3E*`

## Jenkins CI/CD Pipeline Setup

### Step 1: Install Jenkins
1. Download and install Jenkins from [jenkins.io](https://www.jenkins.io/download/)
2. Start Jenkins service
3. Access Jenkins at `http://localhost:8080`
4. Complete the initial setup wizard

### Step 2: Install Required Plugins
Install the following Jenkins plugins:
- Git plugin
- Pipeline plugin
- Docker plugin
- Email Extension plugin

### Step 3: Configure Jenkins Environment
1. Go to **Manage Jenkins** → **System Configuration** → **Global Properties**
2. Add environment variables:
   - `DOTNET_CLI_TELEMETRY_OPTOUT`: `1`
   - `DOTNET_SKIP_FIRST_TIME_EXPERIENCE`: `1`

### Step 4: Set Up Email Configuration
1. Go to **Manage Jenkins** → **System Configuration**
2. Configure **Extended E-mail Notification**:
   - SMTP server: Your email provider's SMTP server
   - Default user e-mail suffix: Your domain
   - Authentication: Enter credentials

### Step 5: Create Pipeline Job
1. Click **New Item** → **Pipeline**
2. Name it "NextHire-v2-Pipeline"
3. In **Pipeline** section:
   - Definition: Pipeline script from SCM
   - SCM: Git
   - Repository URL: `https://github.com/MinhQuan1563/NextHire-v2.git`
   - Script Path: `src/NextHireApp.HttpApi.Host/Jenkinsfile`

### Step 6: Configure Discord Webhook (Optional)
1. Create a Discord server webhook
2. Update the `DISCORD_WEBHOOK` environment variable in the Jenkinsfile
3. Replace the webhook URL with your own

### Pipeline Stages Explained

The Jenkins pipeline includes the following stages:

#### 1. Environment Check
```groovy
stage('Environment Check') {
    steps {
        sh '''
            echo "=== .NET version ==="
            dotnet --version
            echo "=== Docker version ==="
            docker version
        '''
    }
}
```
- Verifies .NET and Docker installations
- Outputs version information for debugging

#### 2. Restore Dependencies
```groovy
stage('Restore') {
    steps {
        dir('src/NextHireApp.HttpApi.Host') {
            sh 'dotnet restore'
        }
    }
}
```
- Restores NuGet packages
- Prepares the project for building

#### 3. Build Application
```groovy
stage('Build') {
    steps {
        dir('src/NextHireApp.HttpApi.Host') {
            sh 'dotnet build --no-restore -c Release'
        }
    }
}
```
- Compiles the application in Release configuration
- Validates code compilation

#### 4. Discord Notification
```groovy
stage('Notify Discord') {
    steps {
        script {
            def commitMessage = sh(script: "git log -1 --pretty=%B", returnStdout: true).trim()
            def commitAuthor  = sh(script: "git log -1 --pretty=%an", returnStdout: true).trim()
            def commitHash    = sh(script: "git rev-parse HEAD", returnStdout: true).trim()
            def githubLink    = "${env.GITHUB_REPO}/commit/${commitHash}"

            def message = """✅ **Build thành công trên branch ${env.BRANCH_NAME ?: 'main'}!**
        • Người commit: ${commitAuthor}
        • Commit: ${commitMessage}
        • Link GitHub: ${githubLink}"""

            def payload = groovy.json.JsonOutput.toJson([content: message])
            writeFile file: 'discord_payload.json', text: payload
            sh "curl -sS -H 'Content-Type: application/json' -X POST --data @discord_payload.json '${env.DISCORD_WEBHOOK}'"
        }
    }
}
```
- Sends build success notification to Discord
- Includes commit information and GitHub links

## Docker Deployment

### Step 1: Understanding the Dockerfile

The project uses a multi-stage Docker build:

```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj"
RUN dotnet publish "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj" \
     -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV TZ=Asia/Ho_Chi_Minh
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NextHireApp.HttpApi.Host.dll"]
```

### Step 2: Build Docker Image
```bash
# Build the Docker image
docker build -t nexthireapp .

# Tag for deployment
docker tag nexthireapp nexthireapp:latest
```

### Step 3: Run Docker Container
```bash
# Run the container
docker run -d \
  --name nexthireapp-container \
  -p 8080:8080 \
  -e ASPNETCORE_URLS=http://+:8080 \
  -e ConnectionStrings__Default="your-connection-string" \
  nexthireapp:latest
```

### Step 4: Verify Deployment
```bash
# Check container status
docker ps

# View logs
docker logs nexthireapp-container

# Access the application
curl http://localhost:8080/health
```

## Monitoring and Notifications

### Email Notifications
The pipeline is configured to send email notifications on build completion:

```groovy
post {
    always {
        script {
            emailext (
                subject: "Jenkins Pipeline: ${env.JOB_NAME} - ${env.BUILD_NUMBER}",
                body: """
                Pipeline execution completed.
                
                Job: ${env.JOB_NAME}
                Build Number: ${env.BUILD_NUMBER}
                Build Status: ${currentBuild.currentResult}
                Build URL: ${env.BUILD_URL}
                
                Check Jenkins for more details.
                """,
                to: "${env.EMAIL}"
            )
        }
    }
}
```

### Discord Integration
Build notifications are sent to Discord with commit information:
- Commit author
- Commit message
- GitHub commit link
- Build status

## Troubleshooting

### Common Issues and Solutions

#### 1. .NET Version Mismatch
**Error**: The current .NET SDK does not support targeting .NET 9.0
**Solution**: 
```bash
# Option 1: Install .NET 9.0 SDK (Recommended)
winget install Microsoft.DotNet.SDK.9
# or download from https://dotnet.microsoft.com/download/dotnet/9.0

# Option 2: Downgrade project to .NET 8.0 (if needed)
# Edit all .csproj files and change:
# <TargetFramework>net9.0</TargetFramework>
# to:
# <TargetFramework>net8.0</TargetFramework>

# Also update Dockerfile base images:
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Verify .NET version
dotnet --version
```

#### 2. Database Connection Issues
**Error**: Cannot connect to database
**Solution**:
1. Verify connection string in `appsettings.json`
2. Ensure database server is running
3. Check firewall settings
4. Verify user permissions

#### 3. SSL Certificate Issues
**Error**: SSL certificate validation failed
**Solution**:
```bash
# Regenerate development certificate
dotnet dev-certs https --clean
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c
dotnet dev-certs https --trust
```

#### 4. Docker Build Failures
**Error**: Docker build fails
**Solution**:
1. Ensure Docker Desktop is running
2. Check available disk space
3. Verify Dockerfile syntax
4. Clear Docker cache: `docker system prune -a`

#### 5. Jenkins Build Failures
**Error**: Jenkins build fails
**Solution**:
1. Check Jenkins logs
2. Verify .NET SDK installation on Jenkins agent
3. Ensure proper permissions
4. Check environment variables

### Performance Optimization

#### 1. Docker Image Optimization
- Use multi-stage builds to reduce image size
- Leverage Docker layer caching
- Use `.dockerignore` to exclude unnecessary files

#### 2. Jenkins Pipeline Optimization
- Use parallel stages where possible
- Cache dependencies between builds
- Use pipeline libraries for reusable components

#### 3. Application Performance
- Enable response caching
- Configure connection pooling
- Use async/await patterns
- Implement proper logging levels

## Conclusion

This tutorial covers the complete setup and deployment of the NextHire-v2 project using ABP Framework with Jenkins CI/CD and Docker. The automated pipeline ensures consistent builds and deployments while providing comprehensive monitoring and notification capabilities.

For additional support and documentation, refer to:
- [ABP Framework Documentation](https://abp.io/docs)
- [Jenkins User Guide](https://www.jenkins.io/doc/)
- [Docker Documentation](https://docs.docker.com/)

---

**Note**: This tutorial is based on the video guide at https://www.youtube.com/watch?v=ZAJwVLaTnOM and provides comprehensive step-by-step instructions for the NextHire-v2 project setup and deployment.