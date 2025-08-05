FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /src

COPY . .
COPY NuGet.Config ./

RUN dotnet restore "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj" --configfile NuGet.Config -nowarn:msb3202,nu1503 --force

RUN dotnet publish "src/NextHireApp.HttpApi.Host/NextHireApp.HttpApi.Host.csproj" --no-restore -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV TZ=Asia/Ho_Chi_Minh
WORKDIR /app

COPY --from=build-env /out /app

ENTRYPOINT ["dotnet", "NextHireApp.HttpApi.Host.dll"]
