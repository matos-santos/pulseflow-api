# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file (supports both .sln and .slnx)
COPY ["*.sln*", "./"]

# Copy project files
COPY ["src/PulseFlow.Api/Api.csproj", "src/PulseFlow.Api/"]
COPY ["src/PulseFlow.Application/Application.csproj", "src/PulseFlow.Application/"]
COPY ["src/PulseFlow.Domain/Domain.csproj", "src/PulseFlow.Domain/"]
COPY ["src/PulseFlow.Infastructure/Infastructure.csproj", "src/PulseFlow.Infastructure/"]
COPY ["tests/PulseFlow.IntegrationTests/PulseFlow.IntegrationTests.csproj", "tests/PulseFlow.IntegrationTests/"]
COPY ["tests/PulseFlow.UnitTests/PulseFlow.UnitTests.csproj", "tests/PulseFlow.UnitTests/"]

# Restore dependencies for the API project
WORKDIR /src/src/PulseFlow.Api
RUN dotnet restore "Api.csproj"

# Copy all source files
WORKDIR /src
COPY . .

# Build the application
WORKDIR /src/src/PulseFlow.Api
RUN dotnet build "Api.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Create a non-root user
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Copy published files
COPY --from=publish /app/publish .

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
	CMD curl -f http://localhost:8080/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "Api.dll"]
