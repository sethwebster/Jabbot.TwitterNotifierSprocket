using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class ResetCommandSprocket : TwitterNotifierAdminCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "reset"; }
        }

        public override bool ExecuteCommand()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://twitterbot.apphb.com/bot/start");
            req.GetResponse();
            return true;
        }
    }
}
