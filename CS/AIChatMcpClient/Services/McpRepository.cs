using ModelContextProtocol.Client;

namespace AIChatMcpClient.Services;

public class McpRepository : IAsyncDisposable {
    public McpClient Client { get; private set; }
    public List<McpClientTool> Tools { get; } = [];
    public List<McpClientResource> Resources { get; } = [];
    public List<McpClientPrompt> Prompts { get; } = [];

    public async Task InitializeAsync() {
        var transport = new HttpClientTransport(new() { Endpoint = new("http://localhost:5002/mcp") });
        Client = await McpClient.CreateAsync(transport);
        var tools = await Client.ListToolsAsync();
        var resources = await Client.ListResourcesAsync();
        var prompts = await Client.ListPromptsAsync();
        Tools.AddRange(tools);
        Resources.AddRange(resources);
        Prompts.AddRange(prompts);
    }

    public async ValueTask DisposeAsync() {
        await Client.DisposeAsync();
    }
}
