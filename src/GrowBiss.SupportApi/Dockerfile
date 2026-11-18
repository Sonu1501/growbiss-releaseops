
# ==========================================
# Stage 1: Build and Publish ASP.NET Core API
# ==========================================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS publish

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project file first to optimize Docker layer caching
COPY ["src/GrowBiss.SupportApi/GrowBiss.SupportApi.csproj", "src/GrowBiss.SupportApi/"]

# Restore NuGet dependencies
RUN dotnet restore "src/GrowBiss.SupportApi/GrowBiss.SupportApi.csproj"

# Copy remaining source code
COPY . .

# Move to API project directory
WORKDIR /src/src/GrowBiss.SupportApi

# Publish application
RUN dotnet publish "GrowBiss.SupportApi.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false


# ==========================================
# Stage 2: Production Runtime
# ==========================================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# ASP.NET Core listens on port 8080 in the container
EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=8080

# Run using the built-in non-root application user
USER $APP_UID

# Copy only the published application
COPY --from=publish /app/publish .

# Start application
ENTRYPOINT ["dotnet", "GrowBiss.SupportApi.dll"]
