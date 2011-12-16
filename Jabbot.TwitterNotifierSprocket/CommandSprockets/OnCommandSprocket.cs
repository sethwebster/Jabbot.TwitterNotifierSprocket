using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class OnCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "on"; }
        }

        public override bool ExecuteCommand()
        {
            var user = Database.FetchOrCreateUser(CurrentMessage.FromUser);
            user.EnableNotifications = true;
            Database.SaveChanges();
            Bot.PrivateReply(CurrentMessage.FromUser, "Twitter notifications have been enabled");
            return true;
        }
    }
}
