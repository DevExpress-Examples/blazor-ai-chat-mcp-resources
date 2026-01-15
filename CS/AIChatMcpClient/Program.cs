using Azure;
using Azure.AI.OpenAI;
using AIChatMcpClient;
using AIChatMcpClient.Components;
using AIChatMcpClient.Services;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

// Read Azure OpenAI settings from appsettings.json
var azureOpenAISettings = new AzureOpenAISettings();
builder.Configuration.GetSection("AzureOpenAI").Bind(azureOpenAISettings);

builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor();

// Register McpRepository as both singleton and hosted service
builder.Services.AddSingleton<McpRepository>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<McpRepository>());

builder.Services.AddSingleton<IChatClient>(sp => {
    var mcpRepository = sp.GetService<McpRepository>();
    var azureOpenAIClient = new AzureOpenAIClient(
        new Uri(azureOpenAISettings.Endpoint),
        new AzureKeyCredential(azureOpenAISettings.ApiKey));
    var chatClient = azureOpenAIClient.GetChatClient(azureOpenAISettings.DeploymentName).AsIChatClient();
    return new ChatClientBuilder(chatClient)
        .ConfigureOptions(co => {
            co.Tools = mcpRepository.Tools.ToArray<AITool>();
        })
        .UseFunctionInvocation()
        .Build();
});

builder.Services.AddDevExpressAI();
var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
} else{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
