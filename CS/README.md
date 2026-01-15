# AI Chat MCP Resources - Configuration Guide

## Configuration Setup

### 1. Create your appsettings.json file

Copy the template file and configure your settings:

```bash
cp AIChatMcpClient/appsettings.json.template AIChatMcpClient/appsettings.json
```

### 2. Configure Azure OpenAI Settings

Edit `appsettings.json` and add your Azure OpenAI credentials:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "your-deployment-name"
  },
  "McpServer": {
    "Endpoint": "http://localhost:5002/mcp"
  }
}
```


