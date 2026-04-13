# See https://aka.ms/customizecontainer to learn how to customize your debug container 
# and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["ZuulRemake/ZuulRemake/ZuulRemake.csproj", "ZuulRemake/ZuulRemake/"]
COPY ["ZuulRemake/ZuulRemake.Tests/ZuulRemake.Tests.csproj", "ZuulRemake/ZuulRemake.Tests/"]

RUN dotnet restore "ZuulRemake/ZuulRemake/ZuulRemake.csproj"

COPY . .

WORKDIR "/src/ZuulRemake/ZuulRemake"
RUN dotnet build -c Release -o /app/build


# Run unit tests
WORKDIR /src/ZuulRemake/ZuulRemake.Tests
RUN dotnet test -c Release --no-build


# This stage is used to publish the service project
WORKDIR /src/ZuulRemake/ZuulRemake
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# Final runtime image
FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ZuulRemake.dll"]
