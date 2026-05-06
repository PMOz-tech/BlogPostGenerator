// See https://aka.ms/new-console-template for more information
using BlogPostGenerator.DependencyInjection;
using BlogPostGenerator.Implementation;
using BlogPostGenerator.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var settings = config.GetSection("AI").Get<AISettings>()!;
var chatClient = ChatClientFactory.Create(settings);
var service = new AIBlogGeneratorService(chatClient);


Console.Write("Enter role (user/assistant/system) [default: user]: ");
var input = Console.ReadLine()?.Trim().ToLower();

var role = input switch
{
    "assistant" => ChatRole.Assistant,
    "system" => ChatRole.System,
    _ => ChatRole.User
};

Console.Write("Enter your prompt: ");
var prompt = Console.ReadLine() ?? string.Empty;

var response = await service.GetResponseAsync(prompt, role);
Console.WriteLine($"\nResponse:\n{response}");


Console.WriteLine("Hello, World!");

