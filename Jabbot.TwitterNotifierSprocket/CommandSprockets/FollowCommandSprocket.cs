using System.Collections.Generic;
using TweetSharp;
using System.Configuration;
using System;

namespace Jabbot.TwitterNotifierSprocket.CommandSprockets
{
    public class FollowCommandSprocket : TwitterNotifierCommandSprocketBase
    {
        public override IEnumerable<string> SupportedCommands
        {
            get { yield return "follow"; }
        }

        public override bool ExecuteCommand()
        {
            if (Arguments.Length == 0)
            {
                throw new InvalidOperationException("Follow who?");
            }
            TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
            svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                ConfigurationManager.AppSettings["User.TokenSecret"]);
            svc.FollowUser(Arguments[0]);
            Bot.PrivateReply(Message.FromUser, "Ok -- I'm following " + Arguments[0]);
            return true;
        }

        private TwitterClientInfo GetClientInfo()
        {
            return new TwitterClientInfo()
            {
                ConsumerKey = ConfigurationManager.AppSettings["Application.ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["Application.ConsumerSecret"]
            };
        }
    }
}
