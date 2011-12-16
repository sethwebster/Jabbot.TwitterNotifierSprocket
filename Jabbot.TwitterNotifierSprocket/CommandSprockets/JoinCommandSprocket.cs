using System;
using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class JoinCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "join"; }
        }

        public override bool ExecuteCommand()
        {
            if (CurrentArguments.Length == 0)
            {
                Bot.PrivateReply(CurrentMessage.FromUser, "You must specify a room to join");
            }
            else
            {
                try
                {
                    Bot.Join(CurrentArguments[0]);
                    Bot.PrivateReply(CurrentMessage.FromUser, "OK - I'm now in " + CurrentArguments[0]);
                }
                catch (Exception e)
                {
                    Bot.PrivateReply(CurrentMessage.FromUser, e.GetBaseException().Message);
                }
            }
            return true;
        }
    }
}
