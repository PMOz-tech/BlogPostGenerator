using Anthropic;
using Anthropic.Models.Messages;
using Microsoft.Extensions.AI;

namespace BlogPostGenerator.Implementation
{
    public class AnthropicChatClient
    {
        private readonly IChatClient _client;

        public AnthropicChatClient(AnthropicClient client)
        {
            _client = client.AsIChatClient("claude-sonnet-4-20250514");
        }

        public async Task<ChatResponse> GetResponseAsync(
            ChatMessage message,
            CancellationToken cancellationToken = default)
        {
            var response = await _client.GetResponseAsync(
                [message],
                cancellationToken: cancellationToken
            );

            return new ChatResponse(new ChatMessage(ChatRole.User, response.Text));
        }
    }
}
