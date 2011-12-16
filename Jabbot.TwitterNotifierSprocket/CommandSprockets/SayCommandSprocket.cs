using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class SayCommandSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "say"; }
        }
        
        public override bool ExecuteCommand()
        {
            Bot.Say(string.Join(" ", CurrentArguments), CurrentMessage.Room);
            return true;
        }
    }
}
