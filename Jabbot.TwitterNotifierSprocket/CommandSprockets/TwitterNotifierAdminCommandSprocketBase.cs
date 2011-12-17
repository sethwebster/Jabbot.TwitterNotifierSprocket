using System.Collections.Generic;
using Jabbot.CommandSprockets;
using Jabbot.TwitterNotifierSprocket.Models;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public abstract class TwitterNotifierAdminCommandSprocketBase : RestrictedCommandSprocket
    {
        protected ITwitterNotifierSprocketRepository Database { get; private set; }

        public TwitterNotifierAdminCommandSprocketBase()
            : this(new TwitterNotifierSprocketRepository())
        {

        }

        public TwitterNotifierAdminCommandSprocketBase(ITwitterNotifierSprocketRepository database)
        {
            Database = database;
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

        
        public override IEnumerable<string> AllowedUserList
        {
            get
            {
                yield return "sethwebster";
            }
        }

        public override IEnumerable<string> BannedUserList
        {
            get { return new string[0]; }
        }
    }

}
