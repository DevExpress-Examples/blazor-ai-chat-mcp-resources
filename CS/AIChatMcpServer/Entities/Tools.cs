using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AIChatMcpServer;

[McpServerToolType]
public class Tools {
    [McpServerTool(Name = "get_time_with_zone"), Description("Gets the current time and time zone")]
    public static string GetTimeWithZone() {
        var now = DateTime.Now;
        var tz = TimeZoneInfo.Local;
        return $"{now:HH:mm} ({tz.DisplayName})";
    }
    
    [McpServerTool(Name = "text_exception"), Description("A test function that always fails with an exception")]
    public static string TestException() {
        throw new InvalidOperationException("This function is designed to fail for demo purposes.");
    }
    
    [McpServerTool(Name = "celsius_to_fahrenheit"), Description("Convers celsius degrees to fahrenheit")]
    public static string CToF([Description("The celsius degrees value")] int value) {
        var valueInFahrenheits = value * 9 / 5 + 32;
        return $"{valueInFahrenheits}\u00b0F ({value}\u00b0C)";
    }
}
