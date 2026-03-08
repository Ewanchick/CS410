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
RUN dotnet restore "ZuulRemake.Tests/ZuulRemake.Tests.csproj"

COPY . .

WORKDIR "/src/ZuulRemake/ZuulRemake"
RUN dotnet build "ZuulRemake.csproj" -c $BUILD_CONFIGURATION -o /app/build


# Run unit tests
FROM build AS test
WORKDIR "/src/ZuulRemake/ZuulRemake.Tests"
RUN dotnet test "ZuulRemake.Tests.csproj" -c Release --no-build --verbosity normal


# This stage is used to publish the service project
FROM test AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/ZuulRemake/ZuulRemake"
RUN dotnet publish "ZuulRemake.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ZuulRemake.dll"]