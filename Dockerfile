# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 5104


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["Application/SampleApi/SampleApi.csproj", "Application/SampleApi/"]
COPY ["Domain/SampleApi.Models/SampleApi.Models.csproj", "Domain/SampleApi.Models/"]
COPY ["Domain/SampleApi.Services/SampleApi.Services.csproj", "Domain/SampleApi.Services/"]
COPY ["Infrastructure/SampleApi.Mongo/SampleApi.Mongo.csproj", "Infrastructure/SampleApi.Mongo/"]
COPY ["Application/SampleApi.Contracts/SampleApi.Contracts.csproj", "Application/SampleApi.Contracts/"]
COPY ["Application/SampleApi.Shared/SampleApi.Shared.csproj", "Application/SampleApi.Shared/"]
RUN dotnet restore "./Application/SampleApi/SampleApi.csproj"
COPY . .
WORKDIR "/src/Application/SampleApi"
RUN dotnet build "./SampleApi.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish

RUN dotnet publish "./SampleApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleApi.dll"]