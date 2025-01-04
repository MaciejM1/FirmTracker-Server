# Step 1: Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["FirmTracker-Server.csproj", "FirmTracker-Server/"]
RUN dotnet restore "FirmTracker-Server/FirmTracker-Server.csproj"

# Copy the rest of the application code
WORKDIR "/src/FirmTracker-Server"
COPY . . 


# Copy the szyfrowanie.dll into the build directory (to ensure it's available during the build)
#COPY ["szyfrowanie.dll", "./"]

# Build the app
RUN dotnet build "FirmTracker-Server.csproj" -c Release -o /app/build

# Step 2: Publish the app
FROM build AS publish
RUN dotnet publish "FirmTracker-Server.csproj" -c Release -o /app/publish

# Step 3: Create the final image using a runtime-only image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy the published app from the previous stage
COPY --from=publish /app/publish .

# Copy the szyfrowanie.dll to the final image (if needed at runtime)
#COPY ["szyfrowanie.dll", "./"]

# Set the entry point for the container
ENTRYPOINT ["dotnet", "FirmTracker-Server.dll"]
