using AIChatMcpServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMcpServer()
       .WithHttpTransport()
       .WithResources<Resources>()
       .WithPrompts<Prompts>()
       .WithTools<Tools>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapMcp("/mcp");

app.Run();
