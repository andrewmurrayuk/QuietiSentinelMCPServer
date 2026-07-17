using System.ComponentModel;
using ModelContextProtocol.Server;

namespace QuietiSentinel.MCPServer.Tools;

/// <summary>
/// Deliberately trivial tools - this server exists solely as a live, whitelisted
/// test fixture for QuietiSentinel.Infrastructure's McpClient (Night 15), not as a
/// real capability. Three tools cover the shapes McpClient needs to prove: no
/// arguments, a single string argument, and multiple typed arguments.
/// </summary>
[McpServerToolType]
public static class TestTools
{
    [McpServerTool, Description("Returns \"pong\". Takes no arguments - the simplest possible tool call.")]
    public static string Ping() => "pong";

    [McpServerTool, Description("Echoes the given message back, unchanged.")]
    public static string Echo(
        [Description("The message to echo back.")] string message)
        => message;

    [McpServerTool, Description("Adds two integers and returns the sum.")]
    public static int AddNumbers(
        [Description("The first number.")] int a,
        [Description("The second number.")] int b)
        => a + b;
}
