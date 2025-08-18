# Hướng Dẫn Chi Tiết Dự Án NextHire-v2

## Video Hướng Dẫn Tham Khảo
**Video**: https://www.youtube.com/watch?v=ZAJwVLaTnOM

Hướng dẫn này cung cấp các bước chi tiết để thiết lập, cấu hình và triển khai dự án NextHire-v2 sử dụng ABP Framework với Jenkins CI/CD và Docker containerization.

## Mục Lục
1. [Yêu Cầu Tiên Quyết](#yêu-cầu-tiên-quyết)
2. [Tổng Quan Dự Án](#tổng-quan-dự-án)
3. [Thiết Lập Môi Trường Phát Triển](#thiết-lập-môi-trường-phát-triển)
4. [Cấu Hình Cơ Sở Dữ Liệu](#cấu-hình-cơ-sở-dữ-liệu)
5. [Thiết Lập Jenkins CI/CD Pipeline](#thiết-lập-jenkins-cicd-pipeline)
6. [Triển Khai Docker](#triển-khai-docker)
7. [Giám Sát Và Thông Báo](#giám-sát-và-thông-báo)
8. [Xử Lý Sự Cố](#xử-lý-sự-cố)

## Yêu Cầu Tiên Quyết

### Phần Mềm Cần Thiết
- **.NET 9.0+ SDK** - [Tải về](https://dotnet.microsoft.com/download/dotnet)
- **Node.js v20.11+** - [Tải về](https://nodejs.org/en)
- **Docker Desktop** - [Tải về](https://www.docker.com/products/docker-desktop)
- **Jenkins** - [Tải về](https://www.jenkins.io/download/)
- **Git** - [Tải về](https://git-scm.com/downloads)
- **Visual Studio Code hoặc Visual Studio** - IDE tùy chọn

### Tài Khoản Cần Thiết
- Tài khoản GitHub để quản lý mã nguồn
- Discord server để nhận thông báo (tùy chọn)
- Tài khoản email để nhận thông báo từ Jenkins

## Tổng Quan Dự Án

NextHire-v2 là ứng dụng web dựa trên ABP Framework với kiến trúc như sau:

### Cấu Trúc Dự Án
```
NextHire-v2/
├── src/
│   ├── NextHireApp.Application/          # Dịch vụ ứng dụng
│   ├── NextHireApp.Application.Contracts/ # Hợp đồng ứng dụng
│   ├── NextHireApp.DbMigrator/           # Công cụ migration database
│   ├── NextHireApp.Domain/               # Thực thể và dịch vụ domain
│   ├── NextHireApp.Domain.Shared/        # Thành phần domain chia sẻ
│   ├── NextHireApp.EntityFrameworkCore/  # Cấu hình EF Core
│   ├── NextHireApp.HttpApi/              # HTTP API controllers
│   ├── NextHireApp.HttpApi.Client/       # HTTP API client
│   └── NextHireApp.HttpApi.Host/         # Web API host
├── Jenkinsfile                           # CI/CD pipeline
├── Dockerfile                            # Cấu hình container
└── README.md                             # Tài liệu dự án
```

### Công Nghệ Chính
- **ABP Framework**: Framework ứng dụng hiện đại cho .NET
- **Entity Framework Core**: Object-relational mapping
- **OpenIddict**: OAuth 2.0/OpenID Connect server
- **Jenkins**: Tích hợp và triển khai liên tục
- **Docker**: Nền tảng containerization

## Thiết Lập Môi Trường Phát Triển

### Thiết Lập Nhanh (Khuyến nghị)

Để thiết lập nhanh tự động, sử dụng các script thiết lập được cung cấp:

```bash
# Linux/macOS
./setup.sh

# Windows
setup.bat
```

Các script này sẽ tự động:
- Kiểm tra phiên bản .NET SDK
- Cài đặt ABP CLI nếu cần
- Khôi phục NuGet packages
- Cài đặt thư viện client-side
- Tạo SSL certificates

### Thiết Lập Thủ Công

Nếu bạn muốn thiết lập thủ công, làm theo các bước sau:

### Bước 1: Clone Repository
```bash
git clone https://github.com/MinhQuan1563/NextHire-v2.git
cd NextHire-v2
```

### Bước 2: Khôi Phục Dependencies
```bash
# Điều hướng đến thư mục dự án chính
cd src/NextHireApp.HttpApi.Host

# Khôi phục .NET packages
dotnet restore

# Cài đặt ABP CLI (nếu chưa cài)
dotnet tool install -g Volo.Abp.Cli

# Cài đặt thư viện client-side
abp install-libs
```

### Bước 3: Cấu Hình Biến Môi Trường
Tạo file `appsettings.Development.json` trong `src/NextHireApp.HttpApi.Host/`:

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

### Bước 4: Tạo SSL Certificate
```bash
# Tạo certificate phát triển cho HTTPS
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c

# Tin tưởng certificate
dotnet dev-certs https --trust
```

## Cấu Hình Cơ Sở Dữ Liệu

### Bước 1: Cấu Hình Connection String
Cập nhật connection string trong `appsettings.json` để phù hợp với thiết lập database của bạn:

```json
{
  "ConnectionStrings": {
    "Default": "Server=your-server;Database=NextHireApp;User Id=your-username;Password=your-password;TrustServerCertificate=true"
  }
}
```

### Bước 2: Chạy Database Migration
```bash
# Điều hướng đến dự án DbMigrator
cd src/NextHireApp.DbMigrator

# Chạy database migrator
dotnet run
```

Điều này sẽ:
- Tạo database nếu chưa tồn tại
- Áp dụng tất cả Entity Framework migrations
- Seed dữ liệu ban đầu bao gồm user admin

### Bước 3: Xác Minh Thiết Lập Database
- User admin mặc định: `admin`
- Mật khẩu admin mặc định: `1q2w3E*`

## Thiết Lập Jenkins CI/CD Pipeline

### Bước 1: Cài Đặt Jenkins
1. Tải và cài đặt Jenkins từ [jenkins.io](https://www.jenkins.io/download/)
2. Khởi động dịch vụ Jenkins
3. Truy cập Jenkins tại `http://localhost:8080`
4. Hoàn thành wizard thiết lập ban đầu

### Bước 2: Cài Đặt Plugins Cần Thiết
Cài đặt các Jenkins plugins sau:
- Git plugin
- Pipeline plugin
- Docker plugin
- Email Extension plugin

### Bước 3: Cấu Hình Môi Trường Jenkins
1. Đi đến **Manage Jenkins** → **System Configuration** → **Global Properties**
2. Thêm biến môi trường:
   - `DOTNET_CLI_TELEMETRY_OPTOUT`: `1`
   - `DOTNET_SKIP_FIRST_TIME_EXPERIENCE`: `1`

### Bước 4: Thiết Lập Cấu Hình Email
1. Đi đến **Manage Jenkins** → **System Configuration**
2. Cấu hình **Extended E-mail Notification**:
   - SMTP server: SMTP server của nhà cung cấp email
   - Default user e-mail suffix: Domain của bạn
   - Authentication: Nhập thông tin đăng nhập

### Bước 5: Tạo Pipeline Job
1. Nhấp **New Item** → **Pipeline**
2. Đặt tên "NextHire-v2-Pipeline"
3. Trong phần **Pipeline**:
   - Definition: Pipeline script from SCM
   - SCM: Git
   - Repository URL: `https://github.com/MinhQuan1563/NextHire-v2.git`
   - Script Path: `src/NextHireApp.HttpApi.Host/Jenkinsfile`

### Bước 6: Cấu Hình Discord Webhook (Tùy Chọn)
1. Tạo Discord server webhook
2. Cập nhật biến môi trường `DISCORD_WEBHOOK` trong Jenkinsfile
3. Thay thế webhook URL bằng URL của bạn

### Giải Thích Các Giai Đoạn Pipeline

Jenkins pipeline bao gồm các giai đoạn sau:

#### 1. Kiểm Tra Môi Trường
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
- Xác minh cài đặt .NET và Docker
- Xuất thông tin phiên bản để debug

#### 2. Khôi Phục Dependencies
```groovy
stage('Restore') {
    steps {
        dir('src/NextHireApp.HttpApi.Host') {
            sh 'dotnet restore'
        }
    }
}
```
- Khôi phục NuGet packages
- Chuẩn bị dự án để build

#### 3. Build Ứng Dụng
```groovy
stage('Build') {
    steps {
        dir('src/NextHireApp.HttpApi.Host') {
            sh 'dotnet build --no-restore -c Release'
        }
    }
}
```
- Biên dịch ứng dụng trong cấu hình Release
- Xác thực việc biên dịch mã

#### 4. Thông Báo Discord
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
- Gửi thông báo build thành công đến Discord
- Bao gồm thông tin commit và link GitHub

## Triển Khai Docker

### Bước 1: Hiểu Về Dockerfile

Dự án sử dụng Docker build đa giai đoạn:

```dockerfile
# Giai đoạn build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj"
RUN dotnet publish "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj" \
     -c Release -o /app/publish

# Giai đoạn runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV TZ=Asia/Ho_Chi_Minh
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NextHireApp.HttpApi.Host.dll"]
```

### Bước 2: Build Docker Image
```bash
# Build Docker image
docker build -t nexthireapp .

# Tag cho triển khai
docker tag nexthireapp nexthireapp:latest
```

### Bước 3: Chạy Docker Container
```bash
# Chạy container
docker run -d \
  --name nexthireapp-container \
  -p 8080:8080 \
  -e ASPNETCORE_URLS=http://+:8080 \
  -e ConnectionStrings__Default="your-connection-string" \
  nexthireapp:latest
```

### Bước 4: Xác Minh Triển Khai
```bash
# Kiểm tra trạng thái container
docker ps

# Xem logs
docker logs nexthireapp-container

# Truy cập ứng dụng
curl http://localhost:8080/health
```

## Giám Sát Và Thông Báo

### Thông Báo Email
Pipeline được cấu hình để gửi thông báo email khi build hoàn thành:

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

### Tích Hợp Discord
Thông báo build được gửi đến Discord với thông tin commit:
- Tác giả commit
- Thông điệp commit
- Link GitHub commit
- Trạng thái build

## Xử Lý Sự Cố

### Các Vấn Đề Thường Gặp Và Giải Pháp

#### 1. Không Khớp Phiên Bản .NET
**Lỗi**: The current .NET SDK does not support targeting .NET 9.0
**Giải pháp**: 
```bash
# Tùy chọn 1: Cài đặt .NET 9.0 SDK (Khuyến nghị)
winget install Microsoft.DotNet.SDK.9
# hoặc tải từ https://dotnet.microsoft.com/download/dotnet/9.0

# Tùy chọn 2: Hạ cấp dự án xuống .NET 8.0 (nếu cần)
# Chỉnh sửa tất cả file .csproj và thay đổi:
# <TargetFramework>net9.0</TargetFramework>
# thành:
# <TargetFramework>net8.0</TargetFramework>

# Cũng cập nhật Dockerfile base images:
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Xác minh phiên bản .NET
dotnet --version
```

#### 2. Vấn Đề Kết Nối Database
**Lỗi**: Cannot connect to database
**Giải pháp**:
1. Xác minh connection string trong `appsettings.json`
2. Đảm bảo database server đang chạy
3. Kiểm tra cài đặt firewall
4. Xác minh quyền người dùng

#### 3. Vấn Đề SSL Certificate
**Lỗi**: SSL certificate validation failed
**Giải pháp**:
```bash
# Tạo lại development certificate
dotnet dev-certs https --clean
dotnet dev-certs https -v -ep openiddict.pfx -p 1d7591a1-1082-438e-9e9a-6e2e8282285c
dotnet dev-certs https --trust
```

#### 4. Lỗi Docker Build
**Lỗi**: Docker build fails
**Giải pháp**:
1. Đảm bảo Docker Desktop đang chạy
2. Kiểm tra dung lượng đĩa có sẵn
3. Xác minh cú pháp Dockerfile
4. Xóa Docker cache: `docker system prune -a`

#### 5. Lỗi Jenkins Build
**Lỗi**: Jenkins build fails
**Giải pháp**:
1. Kiểm tra Jenkins logs
2. Xác minh cài đặt .NET SDK trên Jenkins agent
3. Đảm bảo quyền phù hợp
4. Kiểm tra biến môi trường

### Tối Ưu Hóa Hiệu Suất

#### 1. Tối Ưu Docker Image
- Sử dụng multi-stage builds để giảm kích thước image
- Tận dụng Docker layer caching
- Sử dụng `.dockerignore` để loại trừ files không cần thiết

#### 2. Tối Ưu Jenkins Pipeline
- Sử dụng parallel stages khi có thể
- Cache dependencies giữa các build
- Sử dụng pipeline libraries cho các thành phần tái sử dụng

#### 3. Hiệu Suất Ứng Dụng
- Bật response caching
- Cấu hình connection pooling
- Sử dụng async/await patterns
- Triển khai logging levels phù hợp

## Kết Luận

Hướng dẫn này bao gồm việc thiết lập và triển khai hoàn chỉnh dự án NextHire-v2 sử dụng ABP Framework với Jenkins CI/CD và Docker. Pipeline tự động đảm bảo builds và deployments nhất quán đồng thời cung cấp khả năng giám sát và thông báo toàn diện.

Để được hỗ trợ thêm và tài liệu, tham khảo:
- [Tài liệu ABP Framework](https://abp.io/docs)
- [Hướng dẫn sử dụng Jenkins](https://www.jenkins.io/doc/)
- [Tài liệu Docker](https://docs.docker.com/)

---

**Ghi chú**: Hướng dẫn này dựa trên video hướng dẫn tại https://www.youtube.com/watch?v=ZAJwVLaTnOM và cung cấp hướng dẫn từng bước toàn diện cho việc thiết lập và triển khai dự án NextHire-v2.