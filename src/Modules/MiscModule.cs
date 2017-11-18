using System.Threading.Tasks;
using Discord.Commands;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Net;

namespace DiscordBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _messages;

        [Command("ams")]
        public async Task Ams()
        {
            _messages = BuildMessages();
            await ReplyAsync(_messages["AMS"]);
        }

        [Command("help")]
        public async Task Help()
        {
            _messages = BuildMessages();
            await ReplyAsync(String.Format(_messages["Help"], Context.Message.Author.Id));
        }

        [Command("youtube")]
        public async Task Youtube()
        {
            _messages = BuildMessages();
            await ReplyAsync(String.Format(_messages["Youtube"], Context.Message.Author.Id));
        }

        [Command("shrug")]
        public async Task Shrug()
        {
            _messages = BuildMessages();
            await Context.Message.DeleteAsync();
            await ReplyAsync(_messages["Shrug"]);
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