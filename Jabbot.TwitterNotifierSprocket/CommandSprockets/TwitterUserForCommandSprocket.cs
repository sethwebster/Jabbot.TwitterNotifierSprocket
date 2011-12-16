using System;
using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    class TwitterUserForCommand : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "twitteruserfor"; }
        }

        public override bool ExecuteCommand()
        {

            if (CurrentArguments.Length == 2)
            {
                string forUserName = this.CurrentArguments[0];
                string twitterUserName = this.CurrentArguments[1];
                var userFor = _database.FetchOrCreateUser(forUserName);
                if (userFor != null)
                {
                    userFor.TwitterUserName = twitterUserName;
                    _database.SaveChanges();
                    Bot.PrivateReply(CurrentMessage.FromUser, String.Format("{0}'s twitter user name is now {1}", userFor.JabbrUserName, userFor.TwitterUserName));
                }
                else
                {
                    Bot.PrivateReply(CurrentMessage.FromUser, String.Format("User {0} was not found", CurrentArguments[0]));
                }
            }
            else
            {
                Bot.PrivateReply(CurrentMessage.FromUser, "twitteruserfor [user] [twitterscreenname]");
            }
            return true;
        }
    }
}
