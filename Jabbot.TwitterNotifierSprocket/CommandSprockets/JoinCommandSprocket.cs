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
            if (Arguments.Length == 0)
            {
                Bot.PrivateReply(Message.FromUser, "You must specify a room to join");
            }
            else
            {
                try
                {
                    Bot.JoinRoom(Arguments[0]);
                    Database.JoinRoom(Arguments[0], Message.FromUser);
                    Bot.PrivateReply(Message.FromUser, "OK - I'm now in " + Arguments[0]);
                }
                catch (Exception e)
                {
                    Bot.PrivateReply(Message.FromUser, e.GetBaseException().Message);
                }
            }
            return true;
        }
    }
}
