#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CodeNet.Identity.Api/CodeNet.Identity.Api.csproj", "src/CodeNet.Identity.Api/"]
COPY ["src/CodeNet.EntityFramework.PostgreSQL/CodeNet.EntityFramework.PostgreSQL.csproj", "src/CodeNet.EntityFramework.PostgreSQL/"]
COPY ["src/CodeNet.EntityFramework/CodeNet.EntityFramework.csproj", "src/CodeNet.EntityFramework/"]
COPY ["src/CodeNet.Core/CodeNet.Core.csproj", "src/CodeNet.Core/"]
COPY ["src/CodeNet.Identity/CodeNet.Identity.csproj", "src/CodeNet.Identity/"]
COPY ["src/CodeNet.ExceptionHandling/CodeNet.ExceptionHandling.csproj", "src/CodeNet.ExceptionHandling/"]
COPY ["src/CodeNet.Logging/CodeNet.Logging.csproj", "src/CodeNet.Logging/"]
RUN dotnet restore "./src/CodeNet.Identity.Api/CodeNet.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/CodeNet.Identity.Api"
RUN dotnet build "./CodeNet.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CodeNet.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeNet.Identity.Api.dll"]