using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class MiscModule : ModuleBase<SocketCommandContext>
    {
        [Command("ams")]
        public Task Ams()
            => ReplyAsync(
                "http://streamable.com/gbqjv");

        [Command("help")]
        public Task Help()
            => ReplyAsync(
                "Howdy <@{Context.Message.Author.Id}>, currently only the !year, !program, !ams and !youtube commands are supported, bug Namtsua if you want another feature to be added.");

        [Command("youtube")]
        public Task Youtube()
            => ReplyAsync(
                $"Hey <@{Context.Message.Author.Id}>, check out my Youtube channel! https://www.youtube.com/channel/UC8KGT0uZ19f6XJPUwxlvzPQ");
    }
}