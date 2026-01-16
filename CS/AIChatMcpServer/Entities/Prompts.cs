using ModelContextProtocol.Server;

namespace AIChatMcpServer;

[McpServerPromptType]
public class Prompts {
    [McpServerPrompt(Name = "joke_prompt", Title = "Tell me a clever, work-appropriate joke")]
    public static string JokePrompt() => "Tell me a joke";
    
    [McpServerPrompt(Name = "summary_prompt", Title = "Summarize text")]
    public static string SummaryPrompt() => "Summarize the following text:";
    
    [McpServerPrompt(Name = "email_prompt", Title = "Write an email")]
    public static string EmailPrompt() => "Draft a formal, polite email based on these notes:";
    
    [McpServerPrompt(Name = "brainstorm_prompt", Title = "Brainstorm ideas")]
    public static string BrainstormPrompt() => "Provide 5 unique and diverse ideas for:";
    
    [McpServerPrompt(Name = "writing_prompt", Title = "Fix my writing")]
    public static string WritingPrompt() => "Proofread the following text:";
}
