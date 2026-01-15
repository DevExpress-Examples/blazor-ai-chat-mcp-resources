using AIChatMcpClient.Models;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace AIChatMcpClient.Services;

public class McpRepository : IHostedService, IAsyncDisposable {
    private readonly string _mcpEndpoint;

    public McpClient Client { get; private set; } = null!;
    public List<McpClientTool> Tools { get; } = [];
    public List<McpClientResource> Resources { get; } = [];
    public List<McpClientPrompt> Prompts { get; } = [];
    public List<PromptSuggestion> PromptSuggestions { get; } = [];

    public McpRepository(IConfiguration configuration) {
        _mcpEndpoint = configuration.GetSection("McpServer:Endpoint").Value 
                       ?? throw new InvalidOperationException("McpServer:Endpoint is not configured in appsettings.json");
    }

    public async Task StartAsync(CancellationToken cancellationToken) {
        var transport = new HttpClientTransport(new() { Endpoint = new(_mcpEndpoint) });
        Client = await McpClient.CreateAsync(transport);
        
        var tools = await Client.ListToolsAsync(cancellationToken: cancellationToken);
        var resources = await Client.ListResourcesAsync(cancellationToken: cancellationToken);
        var prompts = await Client.ListPromptsAsync(cancellationToken: cancellationToken);
        
        Tools.AddRange(tools);
        Resources.AddRange(resources);
        Prompts.AddRange(prompts);

        // Preload prompt suggestions at startup
        foreach (var prompt in Prompts) {
            var result = await prompt.GetAsync();
            var content = result.Messages[0].Content;
            PromptSuggestions.Add(new PromptSuggestion {
                PromptMessage = ((TextContentBlock)content).Text,
                Title = prompt.Title ?? "Untitled"
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public async ValueTask DisposeAsync() {
        await Client.DisposeAsync();
    }
}
