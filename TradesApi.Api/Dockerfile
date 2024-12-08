# Use the official image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TradesApi.Api/TradesApi.Api.csproj", "TradesApi.Api/"]
COPY ["TradesApi.Core/TradesApi.Core.csproj", "TradesApi.Core/"]
COPY ["TradesApi.Infrastructure/TradesApi.Infrastructure.csproj", "TradesApi.Infrastructure/"]
RUN dotnet restore "TradesApi.Api/TradesApi.Api.csproj"
COPY . .
WORKDIR "/src/TradesApi.Api"
RUN dotnet build "TradesApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradesApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradesApi.Api.dll"]