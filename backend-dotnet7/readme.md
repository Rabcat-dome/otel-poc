cd otel.models
dotnet ef migrations add ini -o EF/Migrations -s ..\otel.api3\otel.api3.csproj -p .\otel.models.csproj
dotnet ef database update -s ..\otel.api3\otel.api3.csproj -p .\otel.models.csproj