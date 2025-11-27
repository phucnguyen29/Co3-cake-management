FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AuthAPI/AuthAPI.csproj ./AuthAPI/
RUN dotnet restore ./AuthAPI/AuthAPI.csproj

COPY AuthAPI ./AuthAPI
RUN dotnet publish ./AuthAPI/AuthAPI.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthAPI.dll"]
