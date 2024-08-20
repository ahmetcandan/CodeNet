#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/StokTakip/Customer/StokTakip.Customer.Api/StokTakip.Customer.Api.csproj", "src/StokTakip/Customer/StokTakip.Customer.Api/"]
COPY ["src/CodeNet.EntityFramework.InMemory/CodeNet.EntityFramework.InMemory.csproj", "src/CodeNet.EntityFramework.InMemory/"]
COPY ["src/CodeNet.EntityFramework/CodeNet.EntityFramework.csproj", "src/CodeNet.EntityFramework/"]
COPY ["src/CodeNet.Core/CodeNet.Core.csproj", "src/CodeNet.Core/"]
COPY ["src/CodeNet.HealthCheck.Elasticsearch/CodeNet.HealthCheck.Elasticsearch.csproj", "src/CodeNet.HealthCheck.Elasticsearch/"]
COPY ["src/CodeNet.Elasticsearch/CodeNet.Elasticsearch.csproj", "src/CodeNet.Elasticsearch/"]
COPY ["src/CodeNet.HealthCheck.EntityFramework/CodeNet.HealthCheck.EntityFramework.csproj", "src/CodeNet.HealthCheck.EntityFramework/"]
COPY ["src/CodeNet.HealthCheck.MongoDB/CodeNet.HealthCheck.MongoDB.csproj", "src/CodeNet.HealthCheck.MongoDB/"]
COPY ["src/CodeNet.MongoDB/CodeNet.MongoDB.csproj", "src/CodeNet.MongoDB/"]
COPY ["src/CodeNet.HealthCheck.RabbitMQ/CodeNet.HealthCheck.RabbitMQ.csproj", "src/CodeNet.HealthCheck.RabbitMQ/"]
COPY ["src/CodeNet.RabbitMQ/CodeNet.RabbitMQ.csproj", "src/CodeNet.RabbitMQ/"]
COPY ["src/CodeNet.HealthCheck.Redis/CodeNet.HealthCheck.Redis.csproj", "src/CodeNet.HealthCheck.Redis/"]
COPY ["src/CodeNet.Redis/CodeNet.Redis.csproj", "src/CodeNet.Redis/"]
COPY ["src/CodeNet.ExceptionHandling/CodeNet.ExceptionHandling.csproj", "src/CodeNet.ExceptionHandling/"]
COPY ["src/CodeNet.Logging/CodeNet.Logging.csproj", "src/CodeNet.Logging/"]
COPY ["src/CodeNet.HealthCheck/CodeNet.HealthCheck.csproj", "src/CodeNet.HealthCheck/"]
COPY ["src/CodeNet.HttpClient/CodeNet.HttpClient.csproj", "src/CodeNet.HttpClient/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Container/StokTakip.Customer.Container.csproj", "src/StokTakip/Customer/StokTakip.Customer.Container/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Repository/StokTakip.Customer.Repository.csproj", "src/StokTakip/Customer/StokTakip.Customer.Repository/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Abstraction/StokTakip.Customer.Abstraction.csproj", "src/StokTakip/Customer/StokTakip.Customer.Abstraction/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Contract/StokTakip.Customer.Contract.csproj", "src/StokTakip/Customer/StokTakip.Customer.Contract/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Model/StokTakip.Customer.Model.csproj", "src/StokTakip/Customer/StokTakip.Customer.Model/"]
COPY ["src/StokTakip/Customer/StokTakip.Customer.Service/StokTakip.Customer.Service.csproj", "src/StokTakip/Customer/StokTakip.Customer.Service/"]
COPY ["src/CodeNet.BackgroundJob/CodeNet.BackgroundJob.csproj", "src/CodeNet.BackgroundJob/"]
COPY ["src/CodeNet.SignalR/CodeNet.SignalR.csproj", "src/CodeNet.SignalR/"]
COPY ["src/CodeNet.Mapper/CodeNet.Mapper.csproj", "src/CodeNet.Mapper/"]
COPY ["src/CodeNet.Parameters.MongoDB/CodeNet.Parameters.MongoDB.csproj", "src/CodeNet.Parameters.MongoDB/"]
COPY ["src/CodeNet.Parameters/CodeNet.Parameters.csproj", "src/CodeNet.Parameters/"]
COPY ["src/CodeNet.MakerChecker/CodeNet.MakerChecker.csproj", "src/CodeNet.MakerChecker/"]
COPY ["src/CodeNet.StackExchange.Redis/CodeNet.StackExchange.Redis.csproj", "src/CodeNet.StackExchange.Redis/"]
RUN dotnet restore "./src/StokTakip/Customer/StokTakip.Customer.Api/StokTakip.Customer.Api.csproj"
COPY . .
WORKDIR "/src/src/StokTakip/Customer/StokTakip.Customer.Api"
RUN dotnet build "./StokTakip.Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StokTakip.Customer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StokTakip.Customer.Api.dll"]