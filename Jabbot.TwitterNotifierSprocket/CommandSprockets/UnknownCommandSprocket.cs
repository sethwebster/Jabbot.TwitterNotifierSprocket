using System.Collections.Generic;
using System.Reflection;
using Jabbot.CommandSprockets;
using System;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class UnknownCommandSprocket : UnhandledCommandSprocket
    {
        public override bool ExecuteCommand()
        {
            if (!String.IsNullOrEmpty(Command) && !Command.Equals("help", System.StringComparison.OrdinalIgnoreCase))
            {
                Bot.PrivateReply(Message.FromUser, string.Format("I don't know what '{0}' means -- try \"@twitterbot?\" for help", Command));
            }
            else
            {
                Bot.PrivateReply(Message.FromUser, "Jabbot Twitter Sprocket - " + Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
                Bot.PrivateReply(Message.FromUser, string.Format("Status: {0}", TwitterNotifierBotStateManager.IsDisabled ? "Disabled" : "Enabled"));
                Bot.PrivateReply(Message.FromUser, Properties.Resources.HelpText);
            }
            return true;
        }

        public override IEnumerable<string> SupportedInitiators
        {
            get
            {
                yield return "@twitterbot?";
                yield return "twitterbot?";
                yield return "@twitterbot";
            }
        }
    }
}
