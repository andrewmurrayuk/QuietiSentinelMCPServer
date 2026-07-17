using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

// Render sets $PORT at container start; Kestrel must bind to it directly
// (same pattern as GuidanceAssistant's deployment - GuidanceAssistant-SA-v1.4
// Section 13).
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services
    .AddMcpServer(options =>
    {
        options.ServerInfo = new() { Name = "QuietiSentinel.MCPServer", Version = "1.0.0" };
    })
    .WithHttpTransport(options =>
    {
        // Stateless is appropriate here - this server never calls back into the
        // client (no sampling, no elicitation), matching GuidanceAssistant's
        // documented rationale (Section 7.4). Even in stateless mode, responses
        // are still framed as Server-Sent Events ("event: message" / "data: {...}")
        // rather than plain JSON - QuietiSentinel.Infrastructure's McpClient (Night
        // 15) is built to parse that framing, per GuidanceAssistant's own
        // documented build learning (Section 7.4 callout).
        options.Stateless = true;
    })
    .WithToolsFromAssembly();

var app = builder.Build();

// Simple liveness endpoint, separate from /mcp - useful for Render's health
// check and for confirming the container is actually up after a cold start
// (GuidanceAssistant-SA-v1.4 Section 12: Render's free tier sleeps after
// inactivity, first request after sleep takes ~30-60s).
app.MapGet("/", () => Results.Text("QuietiSentinel.MCPServer is running. MCP endpoint: /mcp"));

app.MapMcp("/mcp");

app.Run();
