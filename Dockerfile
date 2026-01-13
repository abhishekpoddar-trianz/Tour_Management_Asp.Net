# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy project file and restore dependencies
COPY DotNetFrameworkProject_CE040_CE087/Tour_Management/Tour_Management.csproj DotNetFrameworkProject_CE040_CE087/Tour_Management/
RUN dotnet restore DotNetFrameworkProject_CE040_CE087/Tour_Management/Tour_Management.csproj

# Copy the rest of the source code
COPY DotNetFrameworkProject_CE040_CE087/Tour_Management/ DotNetFrameworkProject_CE040_CE087/Tour_Management/

# Build the application
WORKDIR /src/DotNetFrameworkProject_CE040_CE087/Tour_Management
RUN dotnet build Tour_Management.csproj -c Release -o /app/build

# Publish the application
RUN dotnet publish Tour_Management.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0

WORKDIR /app

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Create directories for uploads and charts
RUN mkdir -p /app/uploads /tmp/charts && \
    chown -R appuser:appuser /app /tmp/charts

# Copy published application
COPY --from=builder /app/publish .

# Set ownership
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    PORT=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "Tour_Management.dll"]