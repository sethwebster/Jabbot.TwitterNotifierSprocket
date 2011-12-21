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

            if (Arguments.Length == 2)
            {
                string forUserName = this.Arguments[0];
                string twitterUserName = this.Arguments[1];
                var userFor = Database.FetchOrCreateUser(forUserName);
                if (userFor != null)
                {
                    userFor.TwitterUserName = twitterUserName;
                    Database.SaveChanges();
                    Bot.PrivateReply(Message.FromUser, String.Format("{0}'s twitter user name is now {1}", userFor.JabbrUserName, userFor.TwitterUserName));
                }
                else
                {
                    Bot.PrivateReply(Message.FromUser, String.Format("User {0} was not found", Arguments[0]));
                }
            }
            else
            {
                Bot.PrivateReply(Message.FromUser, "twitteruserfor [user] [twitterscreenname]");
            }
            return true;
        }
    }
}
