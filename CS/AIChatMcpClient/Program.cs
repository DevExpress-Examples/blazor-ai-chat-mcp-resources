using Azure;
using Azure.AI.OpenAI;
using AIChatMcpClient;
using AIChatMcpClient.Components;
using AIChatMcpClient.Services;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor();

builder.Services.AddSingleton<McpRepository>();
builder.Services.AddHostedService<McpRepositoryInitHostedService>();

builder.Services.AddSingleton<IChatClient>(sp => {
    var mcpRepository = sp.GetRequiredService<McpRepository>();
    var azureOpenAIClient = new AzureOpenAIClient(
        new Uri(EnvSettings.AzureOpenAIEndpoint),
        new AzureKeyCredential(EnvSettings.AzureOpenAIKey));
    var chatClient = azureOpenAIClient.GetChatClient(EnvSettings.DeploymentName).AsIChatClient();
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
