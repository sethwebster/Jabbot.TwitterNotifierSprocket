using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class StartupSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "startup"; }
        }

        public override bool ExecuteCommand()
        {
            TwitterNotifierBotStateManager.IsDisabled = false;
            Bot.PrivateReply(Message.FromUser, "I have been enabled.");
            return true;
        }
    }
}
