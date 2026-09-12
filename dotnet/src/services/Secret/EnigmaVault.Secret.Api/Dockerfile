FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/Directory.Packages.props", "./"]

COPY ["dotnet/src/services/Secret/EnigmaVault.Secret..Api/EnigmaVault.Secret.Api.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Api/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret..Application/EnigmaVault.Secret.Application.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Application/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret..Infrastructure/EnigmaVault.Secret.Infrastructure.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Infrastructure/"]
COPY ["dotnet/src/services/Secret/EnigmaVault.Secret..Domain/EnigmaVault.Secret.Domain.csproj", "dotnet/src/services/Secret/EnigmaVault.Secret.Domain/"]

COPY ["dotnet/src/shared/infrastructure/Shared.Messaging/Shared.Messaging.csproj", "dotnet/src/shared/infrastructure/Shared.Messaging/"]
COPY ["dotnet/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "dotnet/src/shared/contracts/Shared.Contracts/"]
COPY ["dotnet/src/shared/core/Shared.Application/Shared.Application.csproj", "dotnet/src/shared/core/Shared.Application/"]
COPY ["dotnet/src/shared/core/Shared.Kernel/Shared.Kernel.csproj", "dotnet/src/shared/core/Shared.Kernel/"]
COPY ["dotnet/src/shared/observability/Shared.Logging/Shared.Logging.csproj", "dotnet/src/shared/observability/Shared.Logging/"]

RUN dotnet restore "dotnet/src/services/Secret/EnigmaVault.Secret.Api/EnigmaVault.Secret.Api.csproj"

COPY . .

WORKDIR "/src/dotnet/src/services/Secret/EnigmaVault.Secret.Api"
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Secret.Api.dll"]