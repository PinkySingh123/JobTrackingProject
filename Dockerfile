# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore dependencies
COPY *.csproj .
COPY Application/ Application/
COPY Controllers/ Controllers/
COPY Domain/ Domain/
COPY Infrastructure/ Infrastructure/
COPY Middleware/ Middleware/
COPY Migrations/ Migrations/

COPY Program.cs .
COPY WeatherForecast.cs .
COPY appsettings.json .

RUN dotnet restore

# Build the project
RUN dotnet publish -c Release -o out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "JobTrackingProject.dll"]