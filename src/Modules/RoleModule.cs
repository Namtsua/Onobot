using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;

namespace DiscordBot.Modules
{
    public class RoleModule : ModuleBase<SocketCommandContext>
    {
        private IConfiguration _messages;
        private IConfiguration _keys;
    
        [Command("year")]
        public async Task Year(string year = "")
        {   
            _messages = BuildMessages();
            
            if (year.Length == 0)
            {
                await ReplyAsync(String.Format(_messages["Year Explanation"], Context.Message.Author.Mention));
                return;
            }

            _keys = BuildKeys();
            var user = Context.User;
            var guild = Context.Guild;
            await YearAsync((user as IGuildUser), guild, year);
        }
        public Task Year()
           => ReplyAsync(String.Format(_messages["Year Alternative Explanation"], Context.Message.Author.Id));

        [Command("program")]
        public async Task Program(string program = "")
        {
            _messages = BuildMessages();
            
            if (program.Length == 0)
            {
                await ReplyAsync(String.Format(_messages["Program Explanation"], Context.Message.Author.Mention));
                return;
                
            }

            _keys = BuildKeys();
            var user = Context.User;
            var guild = Context.Guild;
            await ProgramAsync((user as IGuildUser), guild, program);
        }
        public Task Program()
           => ReplyAsync(String.Format(_messages["Program Alternative Explanation"], Context.Message.Author.Id));


        [Command("upgrade")]
        public async Task Upgrade()
        {
            var user = Context.User as SocketGuildUser;
            var roles = (user as IGuildUser).Guild.Roles;
            var firstYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["1st year"]);
            var secondYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["2nd year"]);
            var thirdYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["3rd year"]);
            var fourthYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["4th year"]);
            var fifthYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["5th year"]);
            var sixthYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["6th year"]);
            var seventhYear = roles.FirstOrDefault(x => x.Id.ToString() == _keys["7th year"]);
            var alumni = roles.FirstOrDefault(x => x.Id.ToString() == _keys["Alumni"]);
            
            if (user.Roles.Contains(firstYear))
            {
                await UpgradeAsync(user,firstYear,secondYear);
            }
            else if (user.Roles.Contains(secondYear))
            {
                await UpgradeAsync(user,secondYear,thirdYear);
            }
            else if (user.Roles.Contains(thirdYear))
            {
                await UpgradeAsync(user,thirdYear,fourthYear);
            }
            else if (user.Roles.Contains(fourthYear))
            {
               await UpgradeAsync(user,fourthYear,fifthYear);
            }
            else if (user.Roles.Contains(fifthYear))
            {
               await UpgradeAsync(user,fifthYear,sixthYear);
            }
            else if (user.Roles.Contains(sixthYear))
            {
               await UpgradeAsync(user,sixthYear,seventhYear);
            }
            else if (user.Roles.Contains(seventhYear))
            {
               await UpgradeAsync(user,seventhYear,alumni);
            }

            await ReplyAsync("You've been upgraded!");
        }
    
        private async Task YearAsync(IGuildUser user, SocketGuild guild, string year)
        {
            int tag = int.Parse(year);
            string tagID = "";
            switch(tag)
            {
                case 0: tagID = _keys["Alumni"]; break;
                case 1: tagID = _keys["1st year"]; break;
                case 2: tagID = _keys["2nd year"]; break;
                case 3: tagID = _keys["3rd year"]; break;
                case 4: tagID = _keys["4th year"]; break;
                case 5: tagID = _keys["5th year"]; break;
                case 6: tagID = _keys["6th year"]; break;
                case 7: tagID = _keys["7th year"]; break;
                default: break;
            }   
            string customMessage = findYearMessage(tag);
            await Reply(customMessage);
            var selectedRole = guild.Roles.FirstOrDefault(x => x.Id.ToString() == _keys[tagID]);
            await user.AddRoleAsync(selectedRole);
            return;
            
       }   
       private async Task ProgramAsync(IGuildUser user, SocketGuild guild, string program)
        {
            int tag = int.Parse(program);
            string tagID = "";
            switch(tag)
            {
                case 1: tagID = _keys["Arts"]; break;
                case 2: tagID = _keys["Architecture"]; break;
                case 3: tagID = _keys["Biopsychology"]; break;
                case 4: tagID = _keys["Biotechnology"]; break;
                case 5: tagID = _keys["Computer Science"]; break;
                case 6: tagID = _keys["Engineering"]; break;
                case 7: tagID = _keys["Forestry"]; break;
                case 8: tagID = _keys["Integrated Sciences"]; break;
                case 9: tagID = _keys["Kinesiology"]; break;
                case 10: tagID = _keys["LFS"]; break;
                case 11: tagID = _keys["Music"]; break;
                case 12: tagID = _keys["Pharmacology"]; break;
                case 13: tagID = _keys["Pharmacy"]; break;
                case 14: tagID = _keys["Physics & Astronomy"]; break;
                case 15: tagID = _keys["Political Science"]; break;
                case 16: tagID = _keys["Sauder"]; break;
                case 17: tagID = _keys["Science"]; break;
                case 18: tagID = _keys["Statistics"]; break;
                case 19: tagID = _keys["VISA"]; break;
                case 20: tagID = _keys["Langara Student"]; break;
                case 21: tagID = _keys["UVIC/SFU Spy"]; break;
                case 22: tagID = _keys["High School Student"]; break;
                default: break;
            }   
            string customMessage = findProgramMessage(tag);
            await Reply(customMessage);
            var selectedRole = guild.Roles.FirstOrDefault(x => x.Id.ToString() == tagID);
            await user.AddRoleAsync(selectedRole);
            return;
            
       }   

       private Task Reply(string message)
            => 
                ReplyAsync(message);

        private async Task UpgradeAsync(IGuildUser user, IRole currentRole, IRole nextRole)
        {
            await user.AddRoleAsync(nextRole);
            await user.RemoveRoleAsync(currentRole);
        }
        private string findYearMessage(int year)
        {
            string customMessage = "";
            switch(year)
            {
                case 0: customMessage = _messages["Alumni"]; break;
                case 1: customMessage = _messages["1st year"]; break;
                case 2: customMessage = _messages["2nd year"]; break;
                case 3: customMessage = _messages["3rd year"]; break;
                case 4: customMessage = _messages["4th year"]; break;
                case 5: customMessage = _messages["5th year"]; break;
                case 6: customMessage = _messages["6th year"]; break;
                case 7: customMessage = _messages["7th year"]; break;
                default: break;
            }
            return customMessage;
        }
         private string findProgramMessage(int program)
        {
            string customMessage = "";
            switch(program)
            {
                case 1: customMessage = _messages["Arts"]; break;
                case 2: customMessage = _messages["Architecture"]; break;
                case 3: customMessage = _messages["Biopsychology"]; break;
                case 4: customMessage = _messages["Biotechnology"]; break;
                case 5: customMessage = _messages["Computer Science"]; break;
                case 6: customMessage = _messages["Engineering"]; break;
                case 7: customMessage = _messages["Forestry"]; break;
                case 8: customMessage = _messages["Integrated Sciences"]; break;
                case 9: customMessage = _messages["Kinesiology"]; break;
                case 10: customMessage = _messages["LFS"]; break;
                case 11: customMessage = _messages["Music"]; break;
                case 12: customMessage = _messages["Pharmacology"]; break;
                case 13: customMessage = _messages["Pharmacy"]; break;
                case 14: customMessage = _messages["Physics & Astronomy"]; break;
                case 15: customMessage = _messages["Political Science"]; break;
                case 16: customMessage = _messages["Sauder"]; break;
                case 17: customMessage = _messages["Science"]; break;
                case 18: customMessage = _messages["Statistics"]; break;
                case 19: customMessage = _messages["VISA"]; break;
                case 20: customMessage = _messages["Langara Student"]; break;
                case 21: customMessage = _messages["UVIC/SFU Spy"]; break;
                case 22: customMessage = _messages["High School Student"]; break;
                default: break;
            }

            return customMessage;
        }

        private IConfiguration BuildMessages()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("src/messages.json")
                .Build();
        }
        private IConfiguration BuildKeys()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("src/keys.json")
                .Build();
        }
    }
}