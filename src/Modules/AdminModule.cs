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
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        [Command("nuke")]
        public async Task Nuke(int messageCount)
        {
            if (privilegeCheck(Context.User as SocketGuildUser))
            {
                var messageList = await Context.Channel.GetMessagesAsync(messageCount+1).Flatten();
                await Context.Channel.DeleteMessagesAsync(messageList);
            }
        }

        [Command("purge")]
        public async Task Purge(IGuildUser target, int amount)
        { 
            if (privilegeCheck(Context.User as SocketGuildUser))
            {
                var messageList = await Context.Channel.GetMessagesAsync(100).Flatten();
                var targetMessages = messageList.Where(x => x.Author == target).ToList();
                await Context.Channel.DeleteMessagesAsync(targetMessages);
            }
        }

        [Command("kick")]
        public async Task Kick(IGuildUser target)
        {
            if (privilegeCheck(Context.User as SocketGuildUser))
            {
                await ReplyAsync("Bye bye <@" + target.Id +"> :wave:");
                await target.KickAsync();
            }
        }

        [Command("ban")]
        public async Task Ban(IGuildUser target)
        {
            if (privilegeCheck(Context.User as SocketGuildUser))
            {
                await ReplyAsync("Sayonara <@" + target.Id +"> :wave:");
                await Context.Guild.AddBanAsync(target);
            }
        }

        private bool privilegeCheck(SocketGuildUser user)
        {
            var roles = (user as IGuildUser).Guild.Roles;
            var admin = roles.FirstOrDefault(x => x.Name == "AMS Hack");
            var mod = roles.FirstOrDefault(x => x.Name == "AMS Associate Hacks");
            if (user.Roles.Contains(admin) || user.Roles.Contains(mod))
            {
                return true;
            }

            return false;
        }
    }
}