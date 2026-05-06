namespace BlogPostGenerator.Models;

public class AISettings
{
    public string Provider { get; set; } = "Anthropic";
    public AnthropicSettings Anthropic { get; set; } = new();
    public OpenAISettings OpenAI { get; set; } = new();
}

public class AnthropicSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "claude-sonnet-4-20250514";
}

public class OpenAISettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = "gpt-4o";
}
