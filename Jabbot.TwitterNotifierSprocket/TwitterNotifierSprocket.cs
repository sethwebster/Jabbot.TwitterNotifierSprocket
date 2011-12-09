using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabbot.Sprockets;
using System.Text.RegularExpressions;
using TweetSharp;
using System.Configuration;
using System.Collections.ObjectModel;

namespace Jabbot.TwitterNotifierSprocket
{
    public class TwitterNotifierSprocket : ISprocket
    {
        private static readonly Regex _usernameMatchRegex = new Regex(@"(@\w+)");
        private static readonly string _tweetFormat = "{0}, you were just mentioned by @{1} here http://jabbr.net/#/rooms/{2}.";
        private static ICollection<UserNotificationModel> _userNotifications = new Collection<UserNotificationModel>();

        public bool Handle(Models.ChatMessage message, Bot bot)
        {
            var twitterUsers = GetUserNamesFromMessage(message.Content);
            foreach (var u in twitterUsers)
            {
                if (ShouldNotifyUser(u.ScreenName))
                {
                    TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
                    svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                        ConfigurationManager.AppSettings["User.TokenSecret"]);
                    svc.SendTweet(String.Format(_tweetFormat, u.ScreenName, message.FromUser, message.Room));
                    MarkUserNotified(u.ScreenName);
                }
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

        private TwitterClientInfo GetClientInfo()
        {
            return new TwitterClientInfo()
            {
                ConsumerKey = ConfigurationManager.AppSettings["Application.ConsumerKey"],
                ConsumerSecret = ConfigurationManager.AppSettings["Application.ConsumerSecret"]
            };
        }

        private void MarkUserNotified(string UserName)
        {
            var userRec = _userNotifications.FirstOrDefault(u => u.UserName == UserName);
            if (userRec == null)
            {
                userRec = new UserNotificationModel()
                {
                    UserName = UserName
                };
                _userNotifications.Add(userRec);
            }
            userRec.LastNotification = DateTime.Now;
        }

        private bool ShouldNotifyUser(string UserName)
        {
            return _userNotifications.FirstOrDefault(u => u.UserName == UserName) == null
                || _userNotifications.FirstOrDefault(u => u.UserName == UserName && (DateTime.Now - u.LastNotification).TotalMinutes >= 5) != null;
        }

    }
}