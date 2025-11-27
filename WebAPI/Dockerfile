FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebAPI/WebAPI.csproj ./WebAPI/
RUN dotnet restore ./WebAPI/WebAPI.csproj

COPY WebAPI ./WebAPI
RUN dotnet publish ./WebAPI/WebAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebAPI.dll"]
