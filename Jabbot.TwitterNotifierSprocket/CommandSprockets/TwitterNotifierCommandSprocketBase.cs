using System.Collections.Generic;
using Jabbot.CommandSprockets;
using Jabbot.TwitterNotifierSprocket.Models;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public abstract class TwitterNotifierCommandSprocketBase : CommandSprocket
    {
        protected ITwitterNotifierSprocketRepository Database { get; private set; }

        public TwitterNotifierCommandSprocketBase()
            : this(new TwitterNotifierSprocketRepository())
        {

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

        public TwitterNotifierCommandSprocketBase(ITwitterNotifierSprocketRepository database)
        {
            Database = database;
        }
    }

}
