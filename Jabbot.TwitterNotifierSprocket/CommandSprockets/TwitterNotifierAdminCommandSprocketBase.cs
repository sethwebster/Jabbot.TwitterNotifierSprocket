using System.Collections.Generic;
using Jabbot.CommandSprockets;
using Jabbot.TwitterNotifierSprocket.Models;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public abstract class TwitterNotifierAdminCommandSprocketBase : RestrictedCommandSprocket
    {
        protected ITwitterNotifierSprocketRepository _database;

        public TwitterNotifierAdminCommandSprocketBase()
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

        public TwitterNotifierAdminCommandSprocketBase(ITwitterNotifierSprocketRepository database)
        {
            _database = database;
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
