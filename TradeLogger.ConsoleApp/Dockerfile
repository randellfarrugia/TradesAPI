# Use .NET SDK as the base image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TradeLogger.ConsoleApp/TradeLogger.ConsoleApp.csproj", "TradeLogger.ConsoleApp/"]
RUN dotnet restore "TradeLogger.ConsoleApp/TradeLogger.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/TradeLogger.ConsoleApp"
RUN dotnet build "TradeLogger.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradeLogger.ConsoleApp.csproj" -c Release -o /app/publish

# Use a runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeLogger.ConsoleApp.dll"]