using System.Collections.Generic;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class SayCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "say"; }
        }
        
        public override bool ExecuteCommand()
        {
            Bot.Say(string.Join(" ", Arguments), Message.Room);
            return true;
        }
    }
}
