using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jupiter.Commands
{
    class HelpCommand
    {
        public static Discord.Rest.RestUserMessage helpMenuMessage;
        public static ulong helpmenuMessageId;

        public static async Task DisplayHelpMenu(SocketMessage msg)
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Teal);
            embed.WithTitle("This Is Jupiter's Help Menu");
            embed.AddField("$jupiter say", "Will Repeat what you said after the command. `$jupiter say hello` Will Output `hello`");
            embed.AddField("$users", "Will Output a List of Users in This Server");
            embed.AddField("$delete [value]", "will delete the specified number of messages");
            embed.AddField("$registered_users", "Will Print a List of Users Registered To The System");
            embed.AddField("$utc", "Will Print The UTC Time");
            embed.AddField("$warn [user] for [reason]", "Currentley not working. This Command is Under Technical Maintenance");
            embed.AddField("$menu / $help", "Like Duh");
            helpMenuMessage = await msg.Channel.SendMessageAsync(null, false, embed.Build());
            helpmenuMessageId = helpMenuMessage.Id;
            var rightArrow = new Emoji(@"➡️");
            var watch = new Emoji(@"⌚");
            var musicNotes = new Emoji(@"🎶");
            await helpMenuMessage.AddReactionAsync(watch);
            await helpMenuMessage.AddReactionAsync(musicNotes);
            //await helpMenuMessage.AddReactionAsync(rightArrow);
        }

        public static async Task OnReaction(SocketReaction reaction)
        {
            if (reaction.UserId != 782261217334001674)
            {
                var info = new Emoji(@"ℹ️");
                var watch = new Emoji(@"⌚");
                var musicNotes = new Emoji(@"🎶");
                if (reaction.MessageId == helpmenuMessageId && reaction.Emote.Name == @"⌚")
                {
                    EmbedBuilder timeHelp = new EmbedBuilder();
                    timeHelp.WithColor(Discord.Color.DarkBlue);
                    timeHelp.AddField("$time", "Will Print The Time For The User Who Asked The Time (Only If The User Registered. See Command `$set-tz [timezone]` For More Info");
                    timeHelp.AddField("$time [user mention here]", "Will Print The Time For The Mentioned User (Only If The User Registered. See Command `$set-tz [timezone]` For More Info)");
                    timeHelp.AddField("$time [username here]", "Will print The Time For The username in the message (Only If The User Registered. See Command `$set-tz [timezone]` For More Info)");
                    timeHelp.AddField("$time everyone", "Will Print The Time For Every User Registered in The System. (Only If The User Registered. See Command `$set-tz [timezone]` For More Info)");
                    timeHelp.AddField("$set-tz [timezone]", "Will Register You To The System With The Specified Timezone");
                    await helpMenuMessage.ModifyAsync(msg => msg.Embed = timeHelp.Build());
                    IUser Jupiter = await reaction.Channel.GetUserAsync(782261217334001674);
                    await helpMenuMessage.RemoveAllReactionsAsync();
                    var leftArrow = new Emoji(@"⬅️");
                    helpMenuMessage.AddReactionAsync(info);
                    helpMenuMessage.AddReactionAsync(musicNotes);
                }
                else if (reaction.MessageId == helpmenuMessageId && reaction.Emote.Name == @"ℹ️")
                {
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithColor(Discord.Color.Teal);
                    embed.WithTitle("This Is Jupiter's Help Menu");
                    embed.AddField("$jupiter say", "Will Repeat what you said after the command. `$jupiter say hello` Will Output `hello`");
                    embed.AddField("$users", "Will Output a List of Users in This Server");
                    embed.AddField("$delete [value]", "will delete the specified number of messages");
                    embed.AddField("$registered_users", "Will Print a List of Users Registered To The System");
                    embed.AddField("$utc", "Will Print The UTC Time");
                    embed.AddField("$warn [user] for [reason]", "Currentley not working. This Command is Under Technical Maintenance");
                    embed.AddField("$menu / $help", "Like Duh");
                    await helpMenuMessage.ModifyAsync(msg => msg.Embed = embed.Build());
                    await helpMenuMessage.RemoveAllReactionsAsync();
                    await helpMenuMessage.AddReactionAsync(watch);
                    await helpMenuMessage.AddReactionAsync(musicNotes);
                }
                else if (reaction.Emote.Name == "🎶")
                {
                    EmbedBuilder embed = new EmbedBuilder();
                    embed.WithColor(Discord.Color.Teal);
                    embed.WithTitle("This Is Jupiter's Music Help Menu");
                    embed.AddField("$play [song name]", "Will Play The Specified Song via YouTube");
                    embed.AddField("$scplay [song name]", "Will Play The Specified Song via Sound Cloud");
                    embed.AddField("$quit / $stop", "Will Stop Playing The Song And Will Disconnect The Bot From The Voice Channel");
                    embed.AddField("$pause", "Will Pause The Current Song");
                    embed.AddField("$resume", "Will Resume Playing The Paused Song");
                    await helpMenuMessage.ModifyAsync(msg => msg.Embed = embed.Build());
                    await helpMenuMessage.RemoveAllReactionsAsync();
                    helpMenuMessage.AddReactionAsync(info);
                    await helpMenuMessage.AddReactionAsync(watch);
                }
            }
        }
    }
}
