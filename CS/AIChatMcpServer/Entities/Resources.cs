using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace AIChatMcpServer;

[McpServerResourceType]
public class Resources {
    static string GetFilePath(string fileName) => Path.Combine(AppContext.BaseDirectory, "Data", fileName);
    static string ReadTextResource(string fileName) => File.ReadAllText(GetFilePath(fileName));
    static string ReadBlobResource(string fileName) => Convert.ToBase64String(File.ReadAllBytes(GetFilePath(fileName)));

    [McpServerResource(Name = "Access log", MimeType = "text/plain")]
    public static string LogResource() => ReadTextResource("access.txt");

    [McpServerResource(Name = "AIChat documentation", MimeType = "text/markdown")]
    public static string DocsResource() => ReadTextResource("dxaichat.md");

    [McpServerResource(Name = "Dashboard screenshot", MimeType = "image/jpeg")]
    public static BlobResourceContents DashboardResource() => new() { Blob = ReadBlobResource("dashboard.jpg"), Uri = "test://dashboard.jpg" };
}
