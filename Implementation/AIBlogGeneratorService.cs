using Microsoft.Extensions.AI;

namespace BlogPostGenerator.Implementation;

public class AIBlogGeneratorService
{
    private readonly IChatClient _client;

    public AIBlogGeneratorService(IChatClient client)
    {
        _client = client;
    }

    public async Task<string> GetResponseAsync(string prompt, ChatRole role)
    {
        var response = await _client.GetResponseAsync(
            [new ChatMessage(role, prompt)]
        );
        return response.Text;
    }
}
