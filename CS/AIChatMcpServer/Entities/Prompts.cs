using ModelContextProtocol.Server;

namespace AIChatMcpServer;

[McpServerPromptType]
public class Prompts {
    [McpServerPrompt(Name = "joke_prompt", Title = "Take a break and enjoy a quick laugh")]
    public static string JokePrompt() => "Tell me a joke";
    
    [McpServerPrompt(Name = "summary_prompt", Title = "Summarize text")]
    public static string SummaryPrompt() => "Summarize the following text:";
    
    [McpServerPrompt(Name = "email_prompt", Title = "Write an email")]
    public static string EmailPrompt() => "Format text as a formal email to a client:";
    
    [McpServerPrompt(Name = "brainstorm_prompt", Title = "Brainstorm ideas")]
    public static string BrainstormPrompt() => "Help me brainstorm ideas for:";
    
    [McpServerPrompt(Name = "writing_prompt", Title = "Fix my writing")]
    public static string WritingPrompt() => "Proofread the following text:";
}
