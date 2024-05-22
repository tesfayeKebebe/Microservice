
# FROM mcr.microsoft.com/dotnet/aspnet:8.0  AS base
# WORKDIR /app
# EXPOSE 8080
# EXPOSE 8081

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# ARG BUILD_CONFIGURATION=Release

# COPY . ./
# RUN dotnet restore "PlatformService.csproj"
# RUN dotnet build "PlatformService.csproj" -c $BUILD_CONFIGURATION -0 /app/build

# FROM build AS publish
# ARG BUILD_CONFIGURATION=Release
# RUN dotnet publish "PlatformService.csproj" -c $BUILD_CONFIGURATION -0 /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT [ "dotnet", "PlatformService.dll"]


# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
# WORKDIR /App
# EXPOSE 8080
# EXPOSE 8081

# # Copy everything
# COPY . ./
# # Restore as distinct layers
# RUN dotnet restore
# # Build and publish a release
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:8.0
# WORKDIR /App
# COPY --from=build-env /App/out .
# ENTRYPOINT ["dotnet", "PlatformService.dll"]


FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY . /src
WORKDIR /src
RUN ls
RUN dotnet build "PlatformService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlatformService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlatformService.dll"]



