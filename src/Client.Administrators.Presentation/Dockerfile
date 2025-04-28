# Base stage for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN mkdir -p /home/app/.aspnet/https && chown -R app:app /home/app/.aspnet/https && \
    mkdir -p /keys && chmod -R 777 /keys

USER app
WORKDIR /app
COPY ./certs/.aspnet/https/Client.Administrators.pfx /home/app/.aspnet/https/Client.Administrators.pfx
COPY ["nlog.config", "."]
EXPOSE 8000 8001

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Install Node.js and dependencies
RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    npm install jquery && \
    npm install --save-dev @types/jquery @types/jquery.validation && \
    npm install --save @types/toastr && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Client.Administrators.Presentation/Client.Administrators.Presentation.csproj", "src/Client.Administrators.Presentation/"]
COPY ["src/Client.Administrators.Application/Client.Administrators.Application.csproj", "src/Client.Administrators.Application/"]
RUN dotnet restore "./src/Client.Administrators.Presentation/Client.Administrators.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Client.Administrators.Presentation/"
RUN dotnet build "./Client.Administrators.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "./Client.Administrators.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client.Administrators.Presentation.dll"]
