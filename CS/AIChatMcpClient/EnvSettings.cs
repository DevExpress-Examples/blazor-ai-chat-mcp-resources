using System;

namespace AIChatMcpClient;

public static class EnvSettings {
    public static string AzureOpenAIEndpoint => Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
    public static string AzureOpenAIKey => Environment.GetEnvironmentVariable("AZURE_OPENAI_APIKEY");
    public static string DeploymentName => "GPT4o";
}
