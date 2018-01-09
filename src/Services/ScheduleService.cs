using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Services
{
    public class ScheduleService
    {
         private readonly DiscordSocketClient _discord;
         private readonly CommandService _commands;
         private IServiceProvider _provider;
         private IConfiguration _messages;

        public ScheduleService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands)
        {
            _discord = discord;
            _commands = commands;
            _provider = provider;
            _messages = BuildMessages();

            _discord.MessageReceived += MessageReceived;
        }

        public async Task ScheduleNightChannelOpen(Func<SocketCommandContext, Task> firstAction, Func<SocketCommandContext, Task> secondAction, SocketCommandContext context, DateTime ExecutionTime)
        {
            await Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            await firstAction(context);
            await ScheduleNightChannelClose(secondAction, firstAction, context, DateTime.Now.AddHours(5)); // Close at 5am
        }

        public async Task ScheduleNightChannelClose(Func<SocketCommandContext, Task> firstAction, Func<SocketCommandContext, Task> secondAction, SocketCommandContext context, DateTime ExecutionTime)
        {
            await Task.Delay((int)ExecutionTime.Subtract(DateTime.Now).TotalMilliseconds);
            await firstAction(context);
            await ScheduleNightChannelOpen(secondAction, firstAction, context, DateTime.Now.AddHours(19)); // Open at midnight
        }

        private async Task CreateNightChannel(SocketCommandContext context)
        {
            var nightChannel = await context.Guild.CreateTextChannelAsync(_messages["Night Channel"]);
            await context.Channel.SendMessageAsync(String.Format(_messages["Night Channel Opens"], nightChannel.Id));
        }

        private async Task CloseNightChannel(SocketCommandContext context)
        {
            await context.Channel.SendMessageAsync(_messages["Night Channel Closes"]);
            var nightChannel = context.Guild.Channels.FirstOrDefault(x => x.Name.ToLower() == _messages["Night Channel"]);
            await nightChannel.DeleteAsync();
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            int argPos = 0;
            if (!message.HasStringPrefix("$begin", ref argPos) || message.Author.Id.ToString() != _messages["Night Channel Gatekeeper"]) return;
            
            Func<SocketCommandContext, Task> channelCreation = new Func<SocketCommandContext, Task>(CreateNightChannel);
            Func<SocketCommandContext, Task> channelDeletion = new Func<SocketCommandContext, Task>(CloseNightChannel);

            var context = new SocketCommandContext(_discord, message);
            await ScheduleNightChannelOpen(channelCreation, channelDeletion, context, DateTime.Now.Date.AddDays(1));
        }
        private IConfiguration BuildMessages()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("src/messages.json")
                .Build();
        }
     }
}   