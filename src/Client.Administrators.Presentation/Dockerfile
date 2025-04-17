# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

RUN mkdir -p /home/app/.aspnet/https

COPY ./certs/.aspnet/https/Client.Administrators.pfx /home/app/.aspnet/https/Client.Administrators.pfx

RUN chown -R app:app /home/app/.aspnet/https

RUN mkdir -p /keys && chmod -R 777 /keys

USER app
WORKDIR /app
COPY ["nlog.config", "."]
EXPOSE 8000
EXPOSE 8001


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

RUN apt-get update && \
    apt-get install -y curl gnupg && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    node -v && npm -v

RUN npm install jquery && \
    npm install --save-dev @types/jquery && \
    npm install --save-dev @types/jquery.validation && \
    npm install --save @types/jquery-validation-unobtrusive


COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Client.Administrators.Presentation/Client.Administrators.Presentation.csproj", "src/Client.Administrators.Presentation/"]
COPY ["src/Client.Administrators.Application/Client.Administrators.Application.csproj", "src/Client.Administrators.Application/"]
RUN dotnet restore "./src/Client.Administrators.Presentation/Client.Administrators.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Client.Administrators.Presentation/"
RUN dotnet build "./Client.Administrators.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Client.Administrators.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client.Administrators.Presentation.dll"]