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
            
        }

        [Command("purge")]
        public Task Help()
            => ReplyAsync(
                "Howdy <@{Context.Message.Author.Id}>, currently only the !year, !program, !ams and !youtube commands are supported, bug Namtsua if you want another feature to be added.");

        [Command("kick")]
        public async Task Kick(IGuildUser target)
        {
            var user = Context.User as SocketGuildUser;
            var roles = (user as IGuildUser).Guild.Roles;
            var admin = roles.FirstOrDefault(x => x.Name == "AMS Hack");
            var mod = roles.FirstOrDefault(x => x.Name == "AMS Associate Hacks");
            if (user.Roles.Contains(admin) || user.Roles.Contains(mod))
            {
                await ReplyAsync("Bye bye <@" + user.Id +"> :wave:");
                await target.KickAsync();
            }
        }

        [Command("ban")]
        public async Task Ban(IGuildUser target)
        {
            var user = Context.User as SocketGuildUser;
            var roles = (user as IGuildUser).Guild.Roles;
            var admin = roles.FirstOrDefault(x => x.Name == "AMS Hack");
            var mod = roles.FirstOrDefault(x => x.Name == "AMS Associate Hacks");
            if (user.Roles.Contains(admin) || user.Roles.Contains(mod))
            {
                await ReplyAsync("Sayonara <@" + user.Id +"> :wave:");
                await Context.Guild.AddBanAsync(target);
            }
        }
    }
}