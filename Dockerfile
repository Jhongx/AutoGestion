FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
# El parámetro PublishStaticWebAssets=true fuerza la inclusión de CSS/JS en el output
RUN dotnet publish -c Release -o /app/publish /p:PublishStaticWebAssets=true

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Instalar sqlite3 de forma persistente en la imagen de producción
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
ENV PORT=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "AutoGestion.dll"]