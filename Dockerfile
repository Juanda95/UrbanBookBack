# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["UrbanBook.sln", "."]
COPY ["src/Core/Domain/Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/Application.csproj", "src/Core/Application/"]
COPY ["src/Infraestructure/Persistence/Persistence.csproj", "src/Infraestructure/Persistence/"]
COPY ["src/Infraestructure/ExternalServices/ExternalServices.csproj", "src/Infraestructure/ExternalServices/"]
COPY ["src/Infraestructure/Scheduler/Scheduler.csproj", "src/Infraestructure/Scheduler/"]
COPY ["src/Presentation/UrbanBook/UrbanBook.csproj", "src/Presentation/UrbanBook/"]

# Restore dependencies
RUN dotnet restore "UrbanBook.sln"

# Copy the entire source code
COPY . .

# Build the application
RUN dotnet build "UrbanBook.sln" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "UrbanBook.sln" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port
EXPOSE 5232

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
    CMD dotnet --version || exit 1

# Entry point
ENTRYPOINT ["dotnet", "UrbanBook.dll"]
