=== Dockerfile ===
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution file
COPY TourManagement.sln ./

# Copy project files for dependency resolution
COPY src/TourManagement.Domain/TourManagement.Domain.csproj src/TourManagement.Domain/
COPY src/TourManagement.Application/TourManagement.Application.csproj src/TourManagement.Application/
COPY src/TourManagement.Infrastructure/TourManagement.Infrastructure.csproj src/TourManagement.Infrastructure/
COPY src/TourManagement.Web/TourManagement.Web.csproj src/TourManagement.Web/

# Restore dependencies (cached layer)
RUN dotnet restore

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/src/TourManagement.Web
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-build -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

# Create a non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published application from builder
COPY --from=builder /app/publish .

# Create directories for logs and set permissions
RUN mkdir -p /app/logs && chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false \
    TZ=UTC

# Expose application port
EXPOSE 8080

# Set entrypoint
ENTRYPOINT ["dotnet", "TourManagement.Web.dll"]