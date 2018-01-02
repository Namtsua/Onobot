using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net;
using ProgramEnum;

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
            await RemoveYearsAsync((user as IGuildUser), guild, year);
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
            _keys = BuildKeys();
            var user = Context.User as SocketGuildUser;
            var yearRoles = getYearRoles((user as IGuildUser), _keys);

            for (int i = 0; i < yearRoles.Count - 1; i++)
            {
                var currentRole = yearRoles[i];
                var nextRole = yearRoles[i+1];
                if (user.Roles.Contains(currentRole))
                {
                    await SwapAsync(user,currentRole,nextRole);
                    break;
                }
            }

            await ReplyAsync("You've been upgraded!");
        }

        [Command("downgrade")]
        public async Task Downgrade()
        {
            _keys = BuildKeys();
            var user = Context.User as SocketGuildUser;
            var yearRoles = getYearRoles((user as IGuildUser), _keys);

            for (int i = 1; i < yearRoles.Count; i++)
            {
                var currentRole = yearRoles[i];
                var previousRole = yearRoles[i-1];
                if (user.Roles.Contains(currentRole))
                {
                    await SwapAsync(user,currentRole,previousRole);
                    break;
                }
            }

            await ReplyAsync("You've been downgraded!");
        }

        [Command("graduate")]
        public async Task Graduate()
        {
            _keys = BuildKeys();
            var user = Context.User as SocketGuildUser;
            var yearRoles = getYearRoles((user as IGuildUser), _keys);

            // Short circuit if they're already an Alumni
            if (user.Roles.Contains(yearRoles.Last()))
            {
                return;
            }

            foreach (IRole role in yearRoles)
            {
                if (user.Roles.Contains(role))
                {
                    await user.RemoveRoleAsync(role);
                }
            }

            await user.AddRoleAsync(yearRoles.Last());
            await ReplyAsync("Congratulations on the graduation!!!!!");
        }

        [Command("remove")]
        public async Task Remove(string role="")
        {
            _keys = BuildKeys();  
            var user = Context.User as SocketGuildUser;  
            var roles = user.Guild.Roles;
            var toBeRemoved = roles.FirstOrDefault(x => x.Name.ToLower() == role.ToLower());

            if (user.Roles.Contains(toBeRemoved))
            {
                await user.RemoveRoleAsync(toBeRemoved);
                await ReplyAsync(toBeRemoved.Name + " has been removed!");
            }
        }

        [Command("remove")]
        public async Task Remove(int role=0)
        {
            _keys = BuildKeys();  
            var roleString = ProgramTypeDescription((ProgramEnum.Programs)role);
            var user = Context.User as SocketGuildUser;  
            var roles = user.Guild.Roles;
            var toBeRemoved = roles.FirstOrDefault(x => x.Name.ToLower() == roleString.ToLower());

            if (user.Roles.Contains(toBeRemoved))
            {
                await user.RemoveRoleAsync(toBeRemoved);
                await ReplyAsync(toBeRemoved.Name + " has been removed!");
            }
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
                default: return;
            }   
            string customMessage = findYearMessage(tag);
            await Reply(customMessage);

            var selectedRole = guild.Roles.FirstOrDefault(x => x.Id.ToString() == tagID);
            await user.AddRoleAsync(selectedRole);            
       }   

       private async Task RemoveYearsAsync(IGuildUser user, SocketGuild guild, string year)
       {
            _keys = BuildKeys();
            var sUser = Context.User as SocketGuildUser;
            var yearRoles = getYearRoles(user, _keys);

            foreach (IRole role in yearRoles)
            {
                if (sUser.Roles.Contains(role))
                {
                    await user.RemoveRoleAsync(role);
                }
            }
       }

       private async Task ProgramAsync(IGuildUser user, SocketGuild guild, string program)
        {
            int tag = int.Parse(program);
            string customMessage = findProgramMessage(tag);
            if (string.IsNullOrEmpty(customMessage)) return;

            await Reply(customMessage);
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
                case 11: tagID = _keys["Linguistics"]; break;
                case 12: tagID = _keys["Masters"]; break;
                case 13: tagID = _keys["Music"]; break;
                case 14: tagID = _keys["Pharmacology"]; break;
                case 15: tagID = _keys["Pharmacy"]; break;
                case 16: tagID = _keys["PhD"]; break;
                case 17: tagID = _keys["Physics & Astronomy"]; break;
                case 18: tagID = _keys["Political Science"]; break;
                case 19: tagID = _keys["Sauder"]; break;
                case 20: tagID = _keys["Science"]; break;
                case 21: tagID = _keys["Statistics"]; break;
                case 22: tagID = _keys["VISA"]; break;
                case 23: tagID = _keys["Langara Student"]; break;
                case 24: tagID = _keys["UVIC/SFU Spy"]; break;
                case 25: tagID = _keys["High School Student"]; break;
                case 26: return;
                default: break;
            }   
            var selectedRole = guild.Roles.FirstOrDefault(x => x.Id.ToString() == tagID);
            await user.AddRoleAsync(selectedRole);
            return;
            
       }   

       private Task Reply(string message)
            => 
                ReplyAsync(message);

        private async Task SwapAsync(IGuildUser user, IRole currentRole, IRole nextRole)
        {
            await user.AddRoleAsync(nextRole);
            await user.RemoveRoleAsync(currentRole);
        }

        private List<IRole> getYearRoles(IGuildUser user, IConfiguration keys)
        {
            List<IRole> yearRoles = new List<IRole>();
            var roles = user.Guild.Roles;
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["1st year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["2nd year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["3rd year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["4th year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["5th year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["6th year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["7th year"]));
            yearRoles.Add(roles.FirstOrDefault(x => x.Id.ToString() == keys["Alumni"]));
 
            return yearRoles;
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
                case 11: customMessage = _messages["Linguistics"]; break;
                case 12: customMessage = _messages["Masters"]; break;
                case 13: customMessage = _messages["Music"]; break;
                case 14: customMessage = _messages["Pharmacology"]; break;
                case 15: customMessage = _messages["Pharmacy"]; break;
                case 16: customMessage = _messages["PhD"]; break;
                case 17: customMessage = _messages["Physics & Astronomy"]; break;
                case 18: customMessage = _messages["Political Science"]; break;
                case 19: customMessage = _messages["Sauder"]; break;
                case 20: customMessage = _messages["Science"]; break;
                case 21: customMessage = _messages["Statistics"]; break;
                case 22: customMessage = _messages["VISA"]; break;
                case 23: customMessage = _messages["Langara Student"]; break;
                case 24: customMessage = _messages["UVIC/SFU Spy"]; break;
                case 25: customMessage = _messages["High School Student"]; break;
                case 26: customMessage = _messages["No Program"]; break;
                default: break;
            }

            return customMessage;
        }

        //https://social.msdn.microsoft.com/Forums/vstudio/en-US/562c4b8c-2960-4983-85ea-dcd7c06b6dce/getting-the-description-of-the-enum-value?forum=csharpgeneral
         private string ProgramTypeDescription(Enum ProgramType)
        {
            FieldInfo fi = ProgramType.GetType().GetField(ProgramType.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return ProgramType.ToString();
            }
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