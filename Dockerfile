# Estágio 1: Runtime Linux
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Estágio 2: SDK para Compilação
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# CORREÇÃO AQUI: Copia o arquivo de dentro da pasta para a pasta correspondente no Docker
COPY ["GestaoEventosWorkshops/GestaoEventosWorkshops.csproj", "GestaoEventosWorkshops/"]
RUN dotnet restore "GestaoEventosWorkshops/GestaoEventosWorkshops.csproj"

# Copia o resto dos arquivos do projeto
COPY . .
WORKDIR "/src/GestaoEventosWorkshops"
RUN dotnet build "GestaoEventosWorkshops.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio 3: Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GestaoEventosWorkshops.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio 4: Imagem Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestaoEventosWorkshops.dll"]