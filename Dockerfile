# Use the official .NET 9 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 9 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["EventPlatform.API/EventPlatform.API.csproj", "EventPlatform.API/"]
RUN dotnet restore "EventPlatform.API/EventPlatform.API.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/EventPlatform.API"
RUN dotnet build "EventPlatform.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "EventPlatform.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app

# Copy the published application from the publish stage
COPY --from=publish /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "EventPlatform.API.dll"] 