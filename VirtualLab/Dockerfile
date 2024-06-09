FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Development -o out
# очень интересно, что в докере можно делать слоями создавать докер, 
# использовать одни образы только для того, чтоб использовать их в других образах,
# очень интересно принцип единной ответвтенности здесь реализован (SRP)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_ENVIRONMENT Docker-Development
# Set the entrypoint for the application
ENTRYPOINT ["dotnet", "VirtualLab.dll"]
