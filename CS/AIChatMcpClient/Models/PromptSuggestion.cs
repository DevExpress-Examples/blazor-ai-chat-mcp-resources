using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

namespace AIChatMcpClient.Models;

public class PromptSuggestion {
    public string Title { get; set; }
    public string PromptMessage { get; set; }
    public async static Task<PromptSuggestion> CreateFromMcpAsync(McpClientPrompt prompt) {
        var result = await prompt.GetAsync();
        var content = result.Messages[0].Content;
        return new()
        {
            PromptMessage = ((TextContentBlock)content).Text,
            Title = prompt.Title
        };
    }
}
