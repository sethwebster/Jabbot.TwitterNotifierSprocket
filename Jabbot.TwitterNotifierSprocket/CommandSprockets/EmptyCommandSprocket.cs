using System.Collections.Generic;
using System.Reflection;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class EmptyCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get
            {
                yield return "";
                yield return "help";
            }
        }

        public override bool ExecuteCommand()
        {
            Bot.PrivateReply(Message.FromUser, "Jabbot Twitter Sprocket - " + Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
            Bot.PrivateReply(Message.FromUser, string.Format("Status: {0}", TwitterNotifierBotStateManager.IsDisabled ? "Disabled" : "Enabled"));
            Bot.PrivateReply(Message.FromUser, Properties.Resources.HelpText);
            return true;
        }
    }
}
