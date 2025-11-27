FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebClient/WebClient.csproj ./WebClient/
RUN dotnet restore ./WebClient/WebClient.csproj

COPY WebClient ./WebClient
RUN dotnet publish ./WebClient/WebClient.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebClient.dll"]
