# QuietiSentinel.MCPServer

A minimal, public MCP (Model Context Protocol) test server. Exists solely as a
live, whitelisted test fixture for `QuietiSentinel.Infrastructure`'s `McpClient`


Built with the official `ModelContextProtocol.AspNetCore` SDK, Streamable HTTP
transport, stateless mode - the same proven pattern as the GuidanceAssistant demo
(GuidanceAssistant-SA-v1.4).

## Tools

| Tool | Arguments | Returns |
|---|---|---|
| `ping` | none | `"pong"` |
| `echo` | `message: string` | the same message, unchanged |
| `add_numbers` | `a: int, b: int` | `a + b` |

## Endpoints

- `GET /` - liveness check (plain text, confirms the container is up)
- `POST /mcp` - the MCP Streamable HTTP endpoint

No authentication - this is a public test fixture.

## Deploying to Render

1. Push this repo to GitHub (`QuietiSentinelMCPServer`).
2. In Render: **New → Web Service** → connect the GitHub repo.
3. Environment: **Docker** (Render will detect the `Dockerfile` automatically).
4. No environment variables required - no secrets, no config.
5. Deploy. Render assigns a public URL (e.g. `https://quietisentinelmcpserver.onrender.com`).
6. Confirm it's up: `GET https://<your-app>.onrender.com/` should return the liveness text.
7. The MCP endpoint for `QuietiSentinel.Infrastructure`'s `McpClient` config is:
   `https://<your-app>.onrender.com/mcp`

Note: Render's free tier sleeps the container after inactivity - the first
request after sleep takes roughly 30-60 seconds to respond (same behaviour
GuidanceAssistant documented). Worth knowing before assuming a live test has
failed when it's actually just a cold start.

## Running locally

```
dotnet run
```

Defaults to port 8080 unless the `PORT` environment variable is set. Test with:

```
curl http://localhost:8080/
```
