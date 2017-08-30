using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Reflection;
public class Program
{


    private CommandService commands;
    private DiscordSocketClient client;
    private IServiceProvider services;
    public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

    public async Task MainAsync()
    {
        client = new DiscordSocketClient();
        client.Log += Log;
        commands = new CommandService();
        
        string token = "secret";

        services = new ServiceCollection().BuildServiceProvider();


        await InstallCommandsAsync();

        await client.LoginAsync(TokenType.Bot, token);
        await client.StartAsync();

        // Block until program is closed.
        await Task.Delay(-1);
    }

    public async Task InstallCommandsAsync()
    {
        client.MessageReceived += HandleCommand;

        await commands.AddModulesAsync(Assembly.GetEntryAssembly());
    }

    public async Task HandleCommand(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null) return;

        int argPos = 0;
        if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) return;
        // Create a Command Context
        var context = new CommandContext(client, message);
        // Execute the command. (result does not indicate a return value, 
        // rather an object stating if the command executed successfully)
        var result = await commands.ExecuteAsync(context, argPos, services);
        if (!result.IsSuccess)
            await context.Channel.SendMessageAsync(result.ErrorReason);
    }
    // private async Task MessageReceived(SocketMessage message)
    // {
    //     if (message.Content == "!ping")
    //     {
    //         await message.Channel.SendMessageAsync("Pong!");
    //     }
    // }

    private Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}