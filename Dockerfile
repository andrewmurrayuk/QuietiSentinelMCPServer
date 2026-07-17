# Multi-stage build - SDK image publishes the app, ASP.NET runtime image runs it.
# Mirrors GuidanceAssistant's proven Render deployment pattern
# (GuidanceAssistant-SA-v1.4 Section 13).

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY QuietiSentinel.MCPServer.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render sets $PORT at container start; Program.cs reads it directly via
# Environment.GetEnvironmentVariable("PORT") and binds Kestrel to it.
ENTRYPOINT ["dotnet", "QuietiSentinel.MCPServer.dll"]
