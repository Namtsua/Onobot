using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        [Command("year")]
        public async Task Year(string year = "")
        {
            if (year.Length == 0)
            {
                await ReplyAsync(
                    $"Sorry <@{Context.Message.Author.Id}>, you'll need to enter \"!year\" and a number from 1 to 7 corresponding to your standing (or 0 for Alumni)."
                );
                return;
            }
            await YearAsync(year);
        }
        //public Task Year()
         //   => ReplyAsync(
          //      $"Hi <@{Context.Message.Author.Id}>, let's assign you a year! What year are you in? \n(Type 0 for Alumni or a number from 1 to 7 corresponding to your standing)");

        [Command("help")]
        public Task Help()
            => ReplyAsync(
                $"Howdy <@{Context.Message.Author.Id}>, currently only the !year, !program, !ams and !youtube commands are supported, bug Namtsua if you want another feature to be added.");

        [Command("youtube")]
        public Task Youtube()
            => ReplyAsync(
                //$"Hello, I am a bot called <@{Context.Message.Author.Id}> written in Discord.Net 1.0\n");
                $"Hey <@{Context.Message.Author.Id}>, check out my Youtube channel! https://www.youtube.com/channel/UC8KGT0uZ19f6XJPUwxlvzPQ");
    
        private async Task YearAsync(string year)
        {
            int tag = int.Parse(year);
            string tagName;
            switch(tag)
            {
                case 0: tagName = "Alumni"; break;
                case 1: tagName = "First Year"; break;
                case 2: tagName = "Second Year"; break;
                case 3: tagName = "Third Year"; break;
                case 4: tagName = "Fourth Year"; break;
                case 5: tagName = "Fifth Year"; break;
                case 6: tagName = "Sixth Year"; break;
                case 7: tagName = "Seventh Year"; break;
                default: break;
            }

            
        }
    }
}