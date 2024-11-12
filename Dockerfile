# Use a imagem base ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use a imagem SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Ajuste o caminho abaixo para que corresponda ao caminho correto do seu .csproj no GitHub
COPY [apiHabilidade/RPG API.csproj, "RPG_API/"]
RUN dotnet restore "RPG_API/RPG_API.csproj"
COPY . .
WORKDIR "/src/RPG_API"
RUN dotnet build "RPG_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica a aplicação para o estágio final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RPG_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RPG_API.dll"]
