using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    public class ScheduleService
    {
    //     private readonly DiscordSocketClient _discord;
    //     private readonly CommandService _commands;
    //     private IServiceProvider _provider;
    //     private IConfiguration _keys;

    //     private bool _banned;

    //     public CommandHandlingService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands)
    //     {
    //         _discord = discord;
    //         _commands = commands;
    //         _provider = provider;
    //         _keys = BuildKeys();

    //         _discord.UserJoined += UserJoined;
    //         _discord.UserLeft += UserLeft;
    //         _discord.UserBanned += UserBanned;
    //         _discord.MessageReceived += MessageReceived;
    //     }

    //     public async Task InitializeAsync(IServiceProvider provider)
    //     {
    //         _provider = provider;
    //         await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
    //         // Add additional initialization code here...
    //     }

    //     private async Task MessageReceived(SocketMessage rawMessage)
    //     {
    //         // Ignore system messages and messages from bots
    //         if (!(rawMessage is SocketUserMessage message)) return;
    //         if (message.Source != MessageSource.User) return;

    //         int argPos = 0;
    //         if (!message.HasStringPrefix("^", ref argPos)) return;

    //         var context = new SocketCommandContext(_discord, message);
    //         var result = await _commands.ExecuteAsync(context, argPos, _provider);

    //         if (result.Error.HasValue && 
    //             result.Error.Value != CommandError.UnknownCommand)
    //             await context.Channel.SendMessageAsync(result.ToString());
    //     }

    //     private async Task UserJoined(SocketGuildUser user)
    //     {
    //         var currentChannel = _discord.GetChannel(Convert.ToUInt64(_keys["General Channel"])) as SocketTextChannel;
    //         await currentChannel.SendMessageAsync(String.Format(_keys["Greeting"], user.Id));
    //         await SendDM(user);
    //     }

    //     private async Task UserLeft(SocketGuildUser user)
    //     {
    //         if (_banned == false)
    //         {
    //             var currentChannel = _discord.GetChannel(Convert.ToUInt64(_keys["General Channel"])) as SocketTextChannel;
    //             await currentChannel.SendMessageAsync(String.Format(_keys["Goodbye"], user.Id));
    //         }
    //         _banned = false;
    //     }

    //     private async Task UserBanned(SocketUser user, SocketGuild guild)
    //     {
    //         var currentChannel = _discord.GetChannel(Convert.ToUInt64(_keys["General Channel"])) as SocketTextChannel;
    //         await currentChannel.SendMessageAsync(String.Format(_keys["Banned"], user.Id));
    //         _banned = true;
    //     }

    //     private async Task SendDM(SocketGuildUser user)
    //     {
    //         await user.SendMessageAsync(_keys["DM"]);
    //     }

    //     private IConfiguration BuildKeys()
    //     {
    //         return new ConfigurationBuilder()
    //             .SetBasePath(Directory.GetCurrentDirectory())
    //             .AddJsonFile("src/keys.json")
    //             .Build();
    //     }
     }
}   