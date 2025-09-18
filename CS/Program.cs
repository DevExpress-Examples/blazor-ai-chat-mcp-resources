using Azure;
using Azure.AI.OpenAI;
using AIChatMcpResources;
using AIChatMcpResources.Components;
using AIChatMcpResources.Data;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
       .AddInteractiveServerComponents();

builder.Services.AddDevExpressBlazor();
builder.Services.AddMvc();

var azureOpenAIClient = new AzureOpenAIClient(
    new Uri(EnvSettings.AzureOpenAIEndpoint),
    new AzureKeyCredential(EnvSettings.AzureOpenAIKey));
var chatClient = azureOpenAIClient.GetChatClient(EnvSettings.DeploymentName).AsIChatClient();

builder.Services.AddSingleton(chatClient);
builder.Services.AddDevExpressAI();
builder.Services.AddMcpServer()
       .WithHttpTransport()
       .WithResources<McpResource>();

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

app.MapMcp("/mcp");

app.Run();
