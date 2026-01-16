using System.ComponentModel;
using ModelContextProtocol.Server;

namespace AIChatMcpServer;

[McpServerToolType]
public class Tools {
    [McpServerTool(Name = "get_time_with_zone"), Description("Returns the current local time and timezone")]
    public static string GetTimeWithZone() {
        var now = DateTime.Now;
        var tz = TimeZoneInfo.Local;
        return $"{now:HH:mm} ({tz.DisplayName})";
    }
    
    [McpServerTool(Name = "text_exception"), Description("Throws a deliberate exception for testing purposes")]
    public static string TestException() {
        throw new InvalidOperationException("Intentional exception for test purposes");
    }
    
    [McpServerTool(Name = "celsius_to_fahrenheit"), Description("Converts Celsius to Fahrenheit")]
    public static string CToF([Description("The celsius degrees value")] int value) {
        var valueInFahrenheits = value * 9 / 5 + 32;
        return $"{valueInFahrenheits}\u00b0F ({value}\u00b0C)";
    }
}
