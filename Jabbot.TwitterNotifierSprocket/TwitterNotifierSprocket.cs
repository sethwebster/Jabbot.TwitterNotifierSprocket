using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using Jabbot.Models;
using Jabbot.Sprockets;
using Jabbot.TwitterNotifierSprocket.Models;
using TweetSharp;

namespace Jabbot.TwitterNotifierSprocket
{
    public class TwitterNotifierSprocket : ISprocket
    {
        private static readonly Regex _usernameMatchRegex = new Regex(@"(@\w+)");
        private static readonly string _tweetFormat = "@{0}, you were just mentioned by @{1} here http://jabbr.net/#/rooms/{2}.";

        private bool _isDisabled = false;

        private void DoMigrations()
        {
            // Get the Jabbr connection string
            var connectionString = ConfigurationManager.ConnectionStrings["Jabbr"];

            // Only run migrations for SQL server (Sql ce not supported as yet)
            var settings = new Migrations.Configuration();
            var migrator = new DbMigrator(settings);
            migrator.Update();
        }

        public bool Handle(ChatMessage message, Bot bot)
        {
            try
            {
                using (var _database = new TwitterNotifierSprocketRepository())
                {
                    _database.RecordActivity(message.FromUser);
                    InviteUserIfNeccessary(message.FromUser, bot, _database);
                    var twitterUsers = GetUserNamesFromMessage(message.Content, _database);
                    var user = _database.FetchOrCreateUser(message.FromUser);
                    if (twitterUsers.Count() > 0)
                    {
                        foreach (var u in twitterUsers)
                        {
                            if (ShouldNotifyUser(u.ScreenName, _database))
                            {
                                NotifyUserOnTwitter(message, user, u);
                                _database.MarkUserNotified(u.ScreenName);
                            }
                        }
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                bot.PrivateReply(message.FromUser, e.GetBaseException().Message);
            }
            return false;
        }

        private void NotifyUserOnTwitter(ChatMessage message, User user, TwitterUser u)
        {
            TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
            svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                ConfigurationManager.AppSettings["User.TokenSecret"]);
            svc.SendTweet(String.Format(_tweetFormat,
                u.ScreenName,
                String.IsNullOrEmpty(user.TwitterUserName) ? user.JabbrUserName : user.TwitterUserName, message.Room));

        }

        private void InviteUserIfNeccessary(string forUser, Bot bot, ITwitterNotifierSprocketRepository _database)
        {
            var user = _database.FetchOrCreateUser(forUser);
            if (String.IsNullOrWhiteSpace(user.TwitterUserName) &&
                !user.DisableInvites
                && (DateTime.Now - user.LastInviteDate).TotalDays > 1)
            {
                bot.PrivateReply(forUser, "If you're using Twitter, I can notify you when you're mentioned and away.");
                bot.PrivateReply(forUser, "-- to tell me your Twitter user name use: @twitterbot? twittername [your-twitter-username]");
                bot.PrivateReply(forUser, "-- to prevent me from reminding you about this: @twitterbot? disableinvite");
                user.LastInviteDate = DateTime.Now;
                _database.SaveChanges();
            }
        }

        IEnumerable<TwitterUser> GetUserNamesFromMessage(string message, ITwitterNotifierSprocketRepository _database)
        {
            return _usernameMatchRegex.Matches(message)
                                .Cast<Match>()
                                .Select(g =>
                                    new TwitterUser()
                                    {
                                        ScreenName = _database.FetchOrCreateUser(g.Value.Replace("@", "")).TwitterUserName
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

        private bool ShouldNotifyUser(string UserName, ITwitterNotifierSprocketRepository _database)
        {
            //TODO: once we figure out why notifications seem low, refactor this debug code out
            Console.Write("Should Notify User {0}? ", UserName);
            var user = _database.FetchOrCreateUser(UserName);
            if (user.EnableNotifications)
            {
                if ((DateTime.Now - user.LastNotification).TotalMinutes > 60)
                {
                    Console.Write("1 hour check passed, ");
                    if ((DateTime.Now - user.LastActivity).TotalMinutes > 5)
                    {
                        Console.WriteLine("5 minute activity check passed, all passed.");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("5 minute activity check failed, ");
                    }
                }
                else
                {
                    Console.Write("1 hour check failed, ");
                }
            }
            else
            {
                Console.Write("Notifications are disabled, ");
            }
            Console.WriteLine("Not notifying");
            return false;
            return (user.EnableNotifications &&
                (
                    (DateTime.Now - user.LastNotification).TotalMinutes >= 60 &&
                (DateTime.Now - user.LastActivity).TotalMinutes > 5));
        }
    }
}