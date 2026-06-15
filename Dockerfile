FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY SMB_SERVER.sln ./
COPY SMB.API/SMB.API.csproj SMB.API/
COPY SMB.APPLICATION/SMB.APPLICATION.csproj SMB.APPLICATION/
COPY SMB.DOMAIN/SMB.DOMAIN.csproj SMB.DOMAIN/
COPY SMB.INFRASTRUCTURE/SMB.INFRASTRUCTURE.csproj SMB.INFRASTRUCTURE/

RUN dotnet restore SMB.API/SMB.API.csproj

COPY . .

FROM restore AS publish
RUN dotnet publish SMB.API/SMB.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM restore AS migrations
RUN dotnet tool install --global dotnet-ef --version 10.0.8
ENV PATH="${PATH}:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "ef"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS api
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "SMB.API.dll"]
