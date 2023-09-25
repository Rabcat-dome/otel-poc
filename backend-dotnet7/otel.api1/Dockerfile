#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["otel.api/otel.api.csproj", "otel.api/"]
RUN dotnet restore "otel.api/otel.api.csproj"
COPY . .
WORKDIR "/src/otel.api"
RUN dotnet build "otel.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "otel.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "otel.api.dll"]