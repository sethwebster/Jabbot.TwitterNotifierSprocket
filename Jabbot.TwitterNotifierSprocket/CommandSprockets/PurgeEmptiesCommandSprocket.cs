using System.Collections.Generic;
using System;
using System.Linq;
using System.Data;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class PurgeEmptiesSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "purgeempties"; }
        }

        public override bool ExecuteCommand()
        {
            var usersToDelete = Database.Users.Where(u => string.IsNullOrEmpty(u.TwitterUserName)).ToArray();
            foreach (var u in usersToDelete)
            {
                Database.Users.Remove(u);
            }
            Database.SaveChanges();
            Bot.PrivateReply(Message.FromUser, String.Format("{0} users purged", usersToDelete.Count()));
            return true;
        }
    }
}
