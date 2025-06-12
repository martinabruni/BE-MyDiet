# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Apis/MyDiet.Identity.Api/MyDiet.Identity.Api.csproj", "src/Apis/MyDiet.Identity.Api/"]
COPY ["src/Businesses/MyDiet.Identity.Business/MyDiet.Identity.Business.csproj", "src/Businesses/MyDiet.Identity.Business/"]
COPY ["src/Domains/MyDiet.Identity.Domain/MyDiet.Identity.Domain.csproj", "src/Domains/MyDiet.Identity.Domain/"]
COPY ["src/Domains/MyDiet.Shared.Domain/MyDiet.Shared.Domain.csproj", "src/Domains/MyDiet.Shared.Domain/"]
COPY ["src/Infrastructures/MyDiet.Identity.Infrastructure/MyDiet.Identity.Infrastructure.csproj", "src/Infrastructures/MyDiet.Identity.Infrastructure/"]
COPY ["src/Apis/MyDiet.Shared.Api/MyDiet.Shared.Api.csproj", "src/Apis/MyDiet.Shared.Api/"]
RUN dotnet restore "./src/Apis/MyDiet.Identity.Api/MyDiet.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/Apis/MyDiet.Identity.Api"
RUN dotnet build "./MyDiet.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MyDiet.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyDiet.Identity.Api.dll"]