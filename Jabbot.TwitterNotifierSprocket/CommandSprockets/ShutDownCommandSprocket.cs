using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class ShutDownSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "shutdown"; }
        }

        public override bool ExecuteCommand()
        {

            TwitterNotifierBotStateManager.IsDisabled = true;
            Bot.PrivateReply(CurrentMessage.FromUser, "I have been disabled.");
            return true;
        }
    }
}
