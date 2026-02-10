<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1307851)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# DevExpress Blazor AI Chat Integration with Model Context Protocol

This solution demonstrates how the [DevExpress Blazor AI Chat](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.DxAIChat) component integrates with the [Model Context Protocol (MCP)](https://modelcontextprotocol.io/docs/getting-started/intro). AI models use MCP to securely interact with external data sources, tools, and files. This approach improves context awareness and response accuracy of the model. For example:

- Check the current time and date in any time zone.
- Convert units, such as temperatures from Celsius to Fahrenheit.
- Read project documentation and other local files.
- Provide insights or summaries from real-time data.

You can add new capabilities to the AI through the MCP server without a need to modify the client code.

![DevExpress Blazor AI Chat Integration with MCP](ai-chat-mcp-integration.png)

## Solution Structure

The solution consists of two projects:

- [AIChatMcpServer](CS/AIChatMcpServer): An MCP server that provides tools, resources, and prompts to the client Blazor application.
- [AIChatMcpClient](CS/AIChatMcpClient): A Blazor Server application that hosts the DevExpress AI Chat component and consumes MCP server capabilities.

## Setup and Configuration

To run this example, configure the project dependencies and set up secure authentication for an AI service.

### AI Packages

We use the following versions of Microsoft AI packages in the project:

- [Microsoft.Extensions.AI](https://www.nuget.org/packages/Microsoft.Extensions.AI) | **9.7.1**
- [Microsoft.Extensions.AI.OpenAI](https://www.nuget.org/packages/Microsoft.Extensions.AI.OpenAI/) | **9.7.1-preview.1.25365.4**
- [Azure.AI.OpenAI](https://www.nuget.org/packages/Azure.AI.OpenAI) | **2.2.0-beta.5**

We do not guarantee compatibility or correct operation with higher versions. Refer to the following announcement for additional information: [DevExpress.AIIntegration moves to a stable version](https://supportcenter.devexpress.com/ticket/details/t1292705/devexpress-aiintegration-references-stable-versions-of-microsoft-ai-packages).

### Register AI Service

> [!NOTE]  
> DevExpress AI-powered extensions follow the "bring your own key" principle. DevExpress does not offer a REST API and does not ship any built-in LLMs/SLMs. You need an active Azure/Open AI subscription to obtain the REST API endpoint, key, and model deployment name. These variables must be specified at application startup to register AI clients and enable DevExpress AI-powered Extensions in your application.

This example uses the [Azure OpenAI](https://azure.microsoft.com/en-us/products/ai-foundry/models/openai/) service. For security, secrets are stored in the [appsettings.json](CS/AIChatMcpClient/appsettings.json) file inside the **AIChatMcpClient** project. Update the `AzureOpenAI` section with your Azure OpenAI credentials:

- `Endpoint`: Your Azure OpenAI endpoint
- `ApiKey`: Your Azure OpenAI key
- `DeploymentName`: Azure OpenAI [model ID](https://learn.microsoft.com/en-us/azure/ai-services/openai/concepts/models)

### Run the Solution

The solution implements a client-server pattern:

- The MCP Server runs as a background service at http://localhost:5002/mcp.
- The client opens a web UI with DevExpress AI Chat at http://localhost:5001.

The **AIChatMcpServer** project must run for the **AIChatMcpClient** to function. If you start the solution from Visual Studio, select _AI Chat with MCP_ from the **Startup Item** dropdown. If you run a solution from a command line, ensure the **AIChatMcpServer** project is running before starting the **AIChatMcpClient** project.

## Implementation Details

### MCP Server

The MCP server acts as a bridge between the AI Chat and your data/services. It exposes tools, resources, and prompts, which allow the client to interact with data and services through a standardized interface.

#### Tools

The server exposes three [tools](CS/AIChatMcpServer/Entities/Tools.cs) that AI Chat can invoke:

- `get_time_with_zone`: Returns current time with timezone.
- `celsius_to_fahrenheit`: Temperature conversion utility (from Celsius to Fahrenheit).
- `text_exception`: Simulates a server-side exception for debug and error handling verification.

#### Resources

The server provides access to the following [static resources](CS/AIChatMcpServer/Entities/Resources.cs) that a user can reference during a chat session.

- [Access log](CS/AIChatMcpServer/Data/access.txt): Nginx-style HTTP server logs with errors and requests.
- [AI Chat API Reference](CS/AIChatMcpServer/Data/dxaichat.md): DevExpress Blazor AI Chat component documentation.
- [Screenshot](CS/AIChatMcpServer/Data/dashboard.jpg): A binary image for use with multimodal LLMs.

#### Prompts

The MCP server exposes reusable [prompt templates](CS/AIChatMcpServer/Entities/Prompts.cs) that accept dynamic arguments. These templates provide instant access to common tasks, allowing users to execute complex workflows without manual prompt engineering.

### Chat Client

The the [DevExpress Blazor AI Chat](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.DxAIChat) connects to Azure OpenAI and [loads](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.DxAIChat.Resources) available tools, resources, and prompts from the MCP server at http://localhost:5002/mcp.

```Razor
<div class="main-container">
    <DxAIChat Resources="Resources">
        <PromptSuggestions>
            @foreach (var suggestion in PromptSuggestions){
                <DxAIChatPromptSuggestion PromptMessage="@suggestion.PromptMessage"
                                          Title="@suggestion.Title"
                                          Text="@suggestion.PromptMessage"/>
            }
        </PromptSuggestions>
    </DxAIChat>
</div>

@code{
    IEnumerable<AIChatResource> Resources { get; set; } = [];
    IEnumerable<PromptSuggestion> PromptSuggestions { get; set; } = [];

    protected override async Task OnInitializedAsync() {
        Resources = McpRepository.Resources.Select(x => new AIChatResource(x.Uri, x.Name,
        	              LoadResourceData, x.MimeType, x.Description));
        PromptSuggestions = McpRepository.PromptSuggestions;
        await base.OnInitializedAsync();
    }

    async Task<IList<AIContent>> LoadResourceData(AIChatResource resource, CancellationToken ct) {
        var readResource = await McpRepository.Client.ReadResourceAsync(resource.Uri, cancellationToken: ct);
        return readResource.Contents.ToAIContents();
    }
}
```

When you interact with the chat:

- Messages go to Azure OpenAI.
- The model automatically identifies and invokes relevant MCP tools.
- Results are displayed in the AI Chat component.

A connection between MCP client and an MCP server endpoint is managed in [McpRepository.cs](CS/AIChatMcpClient/Services/McpRepository.cs). The service initializes an McpClient on startup, loads available tools, resources, and prompts from the MCP server, and preloads prompt suggestions. The class implements `IHostedService` for lifecycle management and `IAsyncDisposable` for proper cleanup of the connection.

## Files to Review

- [Prompts.cs](CS/AIChatMcpServer/Entities/Prompts.cs)
- [Resources.cs](CS/AIChatMcpServer/Entities/Resources.cs)
- [Tools.cs](CS/AIChatMcpServer/Entities/Tools.cs)
- [AIChatMcpServer/Program.cs](CS/AIChatMcpServer/Program.cs)
- [Index.razor](CS/AIChatMcpClient/Components/Pages/Index.razor)
- [McpRepository.cs](CS/AIChatMcpClient/Services/McpRepository.cs)
- [AIChatMcpClient/Program.cs](CS/AIChatMcpClient/Program.cs)
- [appsettings.json](CS/AIChatMcpClient/appsettings.json)

## Documentation

- [DxAIChat](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.DxAIChat)
- [Resources](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.DxAIChat.Resources)
- [AIChatResource](https://docs.devexpress.com/Blazor/DevExpress.AIIntegration.Blazor.Chat.AIChatResource)

<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-ai-chat-mcp-resources&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=blazor-ai-chat-mcp-resources&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
