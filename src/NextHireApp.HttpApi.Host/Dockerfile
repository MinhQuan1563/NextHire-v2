# # stage build
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# WORKDIR /src

# # sao chép toàn bộ mã nguồn và khôi phục package
# COPY . .
# RUN dotnet restore "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj"

# # publish ứng dụng ra thư mục /app/publish
# RUN dotnet publish "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj" \
#      -c Release -o /app/publish

# # stage runtime
# FROM mcr.microsoft.com/dotnet/aspnet:9.0
# ENV TZ=Asia/Ho_Chi_Minh
# WORKDIR /app
# COPY --from=build /app/publish .
# # cổng sẽ được lấy từ biến môi trường ASPNETCORE_URLS hoặc PORT
# ENTRYPOINT ["dotnet", "NextHireApp.HttpApi.Host.dll"]

FROM jksnexthireacr.azurecr.io/jenkins:jdk21

USER root

RUN mkdir -p /var/jenkins_home/workspace \
    && chown -R jenkins:jenkins /var/jenkins_home/workspace

RUN git config --global --add safe.directory /var/jenkins_home/workspace \
    && git config --global --add safe.directory /var/jenkins_home/workspace/MyNextHire@script

USER jenkins
