FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet publish "otel.api3/otel.api3.csproj" -c Release -o /build/publish

FROM base AS publish
RUN apt-get update \
  && rm -rf /var/lib/apt/lists/*
COPY --from=build /build/publish .
ENTRYPOINT ["dotnet", "otel.api3.dll"]