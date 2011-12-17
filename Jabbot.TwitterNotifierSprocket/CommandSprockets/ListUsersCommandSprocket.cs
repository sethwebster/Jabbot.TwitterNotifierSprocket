using System;
using System.Collections.Generic;
using System.Linq;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class ListUsersCommandSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "listusers"; }
        }

        public override bool ExecuteCommand()
        {
            var users = Database.Users.OrderBy(u => u.JabbrUserName).ToArray().Select(
                   u => String.Format(
                       "{0} <{1}>", u.JabbrUserName,
                       string.IsNullOrEmpty(u.TwitterUserName) ? "empty" : "@" + u.TwitterUserName)
                   );
            Bot.PrivateReply(CurrentMessage.FromUser, String.Join(Environment.NewLine, users.ToArray()));
            return true;
        }
    }
}
