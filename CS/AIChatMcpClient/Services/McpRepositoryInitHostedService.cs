namespace AIChatMcpClient.Services;

public class McpRepositoryInitHostedService(McpRepository repository) : IHostedService {
    public async Task StartAsync(CancellationToken ct) {
        await repository.InitializeAsync();
    }

    public Task StopAsync(CancellationToken ct) => Task.CompletedTask;
}
