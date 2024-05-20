FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release 
WORKDIR /src
COPY ["./VirtualLab.csproj", "VirtualLab/"]
RUN dotnet restore "VirtualLab/VirtualLab.csproj"
COPY . .
WORKDIR "/src/VirtualLab"
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build
# swagger на deve поэтому не Release
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "VirtualLab.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VirtualLab.dll"]
