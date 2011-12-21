using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class DisableInviteCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "disableinvite"; }
        }

        public override bool ExecuteCommand()
        {
            var user = Database.FetchOrCreateUser(Message.FromUser);
            user.DisableInvites = true;
            Database.SaveChanges();
            Bot.PrivateReply(Message.FromUser, "OK - I'll leave you alone.");
            return true;
        }
    }
}
