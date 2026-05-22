
# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:10.0-nanoserver-ltsc2022 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GestaoEventosWorkshops/GestaoEventosWorkshops.csproj", "GestaoEventosWorkshops/"]
RUN dotnet restore "./GestaoEventosWorkshops/GestaoEventosWorkshops.csproj"
COPY . .
WORKDIR "/src/GestaoEventosWorkshops"
RUN dotnet build "./GestaoEventosWorkshops.csproj" -c %BUILD_CONFIGURATION% -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GestaoEventosWorkshops.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GestaoEventosWorkshops.dll"]