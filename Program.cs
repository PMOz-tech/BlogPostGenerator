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


Console.Write("What topic are you interested in? \n");
var topic = Console.ReadLine()?.Trim().ToLower();

//var role = input switch
//{
//    "assistant" => ChatRole.Assistant,
//    "system" => ChatRole.System,
//    _ => ChatRole.User
//};

var role = ChatRole.User;

Console.Write("What tone would you like? (Friendly, Professional, Funny) \n");
var tone = Console.ReadLine() ?? string.Empty;

Console.Write("What's your preferred lenght? (short, medium or long) \n");
var lenght = Console.ReadLine() ?? string.Empty;

var prompt = $"Take topic - {topic} and generate a blog outline with bullet points and outline with section headers and no intro text " +
    $"Tone :- {tone}." +
    $"Length :- {lenght} (short=3 sections, medium=5, long=7)";


var outline = await service.GetResponseAsync(prompt, role);
Console.WriteLine($"\nResponse:\n{outline}");


var secondPrpmpt = $"Use this {outline} to generate a full blog post." +
    $"These are the rules :- " +
    $"1. Tone - {tone}" +
    $"2. The blog should have an interesting headline" +
    $"3. The blog should have an opening" +
    $"4. The blog should have an epic conclusion" +
    $"5. Keep the paragraph short and in line with the {topic} and {tone}" +
    $"6. Keep paragraphs short (3-4 sentences max)" +
    $"7.Write in markdown format with headers";

var fullPost = await service.GetResponseAsync(secondPrpmpt, role);
Console.WriteLine("\n--- FULL POST ---");
Console.WriteLine(fullPost);
Console.WriteLine("Hello, World!");

var thirdPrompt = $"Write three social media variations of this using this {fullPost}." +
    $"These are the social media " +
    $"- X (exactly 280 character limit)" +
    $"- LinkedIn Post (if the tone {tone} set is not professional, make it professional and the paragraph should be at most 4)" +
    $"- Instagram - (200 character lenght and the tone should be casual)" +
    $"" +
    $"Label each of the variants accordingly.";

var blogVariants = await service.GetResponseAsync(thirdPrompt, role);

Console.WriteLine(blogVariants);

var outputPath = "output/outline.md";
Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
await File.WriteAllTextAsync(outputPath, outline);

var fullpostPath = "output/full-post.md";
Directory.CreateDirectory(Path.GetDirectoryName(fullpostPath)!);
await File.WriteAllTextAsync(outputPath, fullpostPath);

var blogVairiantsPath = "output/social-posts.md";
Directory.CreateDirectory(Path.GetDirectoryName(blogVairiantsPath)!);
await File.WriteAllTextAsync(outputPath, blogVariants);


Console.WriteLine("\nSaved to /output folder!");

