#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["otel.api3/otel.api3.csproj", "otel.api3/"]
RUN dotnet restore "otel.api3/otel.api3.csproj"
COPY . .
WORKDIR "/src/otel.api3"
RUN dotnet build "otel.api3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "otel.api3.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "otel.api3.dll"]