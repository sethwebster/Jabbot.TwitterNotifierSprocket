using System.Collections.Generic;
using System;
using System.Linq;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class SayInCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "sayin"; }
        }

        public override bool ExecuteCommand()
        {
            if (Arguments.Length == 0)
                throw new InvalidOperationException("You must provide a room name");
            string room = Arguments[0];
            Bot.Say(string.Join(" ", Arguments.Skip(1).ToArray()), room);
            return true;
        }
    }
}
