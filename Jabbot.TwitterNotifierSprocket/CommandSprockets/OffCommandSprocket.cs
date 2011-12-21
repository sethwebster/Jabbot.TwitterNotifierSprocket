using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class OffCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "off"; }
        }

        public override bool ExecuteCommand()
        {
            var user = Database.FetchOrCreateUser(Message.FromUser);
            user.EnableNotifications = false;
            Database.SaveChanges();
            Bot.PrivateReply(Message.FromUser, "Twitter notifications have been enabled");
            return true;
        }
    }
}
