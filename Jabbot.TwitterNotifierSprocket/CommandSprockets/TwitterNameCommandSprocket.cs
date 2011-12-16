﻿using System;
using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class TwitterNameCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get
            {
                yield return "twittername";
            }
        }

        public override bool ExecuteCommand()
        {
            if (!HasArguments)
            {
                var user = Database.FetchOrCreateUser(CurrentMessage.FromUser);
                Bot.PrivateReply(CurrentMessage.FromUser, String.Format("Your Twitter user is {0}",
                    String.IsNullOrEmpty(user.TwitterUserName) ? "<empty>" : user.TwitterUserName));

            }
            else
            {
                Database.SetTwitterUserName(CurrentMessage.FromUser, CurrentArguments[0]);
                Bot.PrivateReply(CurrentMessage.FromUser, String.Format("Your Twitter user name is now {0}", CurrentArguments[0]));
            }
            return true;
        }
    }
}
