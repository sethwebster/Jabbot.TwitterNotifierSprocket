using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabbot.Sprockets;
using System.Text.RegularExpressions;
using TweetSharp;
using System.Configuration;

namespace Jabbot.TwitterNotifierSprocket
{
    public class TwitterNotifierSprocket : ISprocket
    {
        private static readonly Regex _usernameMatchRegex = new Regex(@"(@\w+)");
       
        private static readonly string _tweetFormat = "{0}, you were just mentioned by @{1} here http://jabbr.net/#/rooms/{2}.";

        public bool Handle(Models.ChatMessage message, Bot bot)
        {
            var twitterUsers = GetUserNamesFromMessage(message.Content);
            foreach (var u in twitterUsers)
            {
                TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
                svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                    ConfigurationManager.AppSettings["User.TokenSecret"]);
                svc.SendTweet(String.Format(_tweetFormat, u.ScreenName, message.FromUser, message.Room));
            }
            return true;
        }

        IEnumerable<TwitterUser> GetUserNamesFromMessage(string message)
        {
            return _usernameMatchRegex.Match(message)
                                .Groups
                                .Cast<Group>()
                                .Skip(1)
                                .Select(g =>
                                    new TwitterUser()
                                    {
                                        ScreenName = g.Value
                                    })
                                .Where(v => !String.IsNullOrEmpty(v.ScreenName));
        }

        TwitterClientInfo GetClientInfo()
        {
            return new TwitterClientInfo()
            {
                ConsumerKey = ConfigurationManager.AppSettings["Application.ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["Application.ConsumerSecret"]
            };
        }
    }
}