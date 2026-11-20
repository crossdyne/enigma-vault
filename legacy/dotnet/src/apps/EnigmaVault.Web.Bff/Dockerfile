FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["dotnet/Directory.Packages.props", "./"]

COPY ["dotnet/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj", "dotnet/src/apps/EnigmaVault.Web.Bff/"]

COPY ["dotnet/src/clients/EnigmaVault.Authentication.Client/EnigmaVault.Authentication.Client.csproj", "dotnet/src/clients/EnigmaVault.Authentication.Client/"]
COPY ["dotnet/src/shared/contracts/Shared.Contracts/Shared.Contracts.csproj", "dotnet/src/shared/contracts/Shared.Contracts/"]
COPY ["dotnet/src/shared/infrastructure/Shared.Http/Shared.Http.csproj", "dotnet/src/shared/infrastructure/Shared.Http/"]
COPY ["dotnet/src/shared/infrastructure/Shared.Redis/Shared.Redis.csproj", "dotnet/src/shared/infrastructure/Shared.Redis/"]
COPY ["dotnet/src/shared/observability/Shared.Logging/Shared.Logging.csproj", "dotnet/src/shared/observability/Shared.Logging/"]

RUN dotnet restore "dotnet/src/apps/EnigmaVault.Web.Bff/EnigmaVault.Web.Bff.csproj"

COPY . .

WORKDIR "/src/dotnet/src/apps/EnigmaVault.Web.Bff"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0  AS runtime
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EnigmaVault.Web.Bff.dll"]