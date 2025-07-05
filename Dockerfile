# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything
COPY . .

# Restore dependencies
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 5000
EXPOSE 5001

# Optional: Tell ASP.NET Core to listen on these ports
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "JobTrackingProject.dll"]