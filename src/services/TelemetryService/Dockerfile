FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["TelemetryService.csproj", "./"]
RUN dotnet restore "TelemetryService.csproj"
COPY . .
RUN dotnet publish "TelemetryService.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TelemetryService.dll"]