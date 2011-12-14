﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabbot.Sprockets;
using System.Text.RegularExpressions;
using TweetSharp;
using System.Configuration;
using System.Collections.ObjectModel;
using Jabbot.TwitterNotifierSprocket.Models;
using Jabbot.Models;
using System.Reflection;
using System.Data.Entity;
using System.Data.Entity.Migrations;

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
           
                    CommandManager mgr = new CommandManager(message, bot, _database);
                    if (!mgr.HandleCommand())
                    {
                        var twitterUsers = GetUserNamesFromMessage(message.Content,_database);
                        var user = _database.FetchOrCreateUser(message.FromUser);
                        if (twitterUsers.Count() > 0)
                        {
                            foreach (var u in twitterUsers)
                            {
                                if (ShouldNotifyUser(u.ScreenName, _database))
                                {
                                    TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
                                    svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                                        ConfigurationManager.AppSettings["User.TokenSecret"]);
                                    svc.SendTweet(String.Format(_tweetFormat, u.ScreenName, String.IsNullOrEmpty(user.TwitterUserName) ? user.JabbrUserName : user.TwitterUserName, message.Room));
                                    _database.MarkUserNotified(u.ScreenName);
                                }
                            }
                            return true;
                        }
                    }
                    
                }
            }
            catch (CommandException ce)
            {
                bot.PrivateReply(message.FromUser, ce.Message);
            }
            catch (Exception e)
            {
                bot.PrivateReply(message.FromUser, e.GetBaseException().Message);
            }
            return false;
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
            var user = _database.Users.FirstOrDefault(u => u.JabbrUserName == UserName);
            return user == null || (user.EnableNotifications &&
                ((DateTime.Now - user.LastNotification).TotalMinutes >= 60 &&
                (DateTime.Now - user.LastActivity).TotalMinutes > 5));
        }
    }

    public class CommandException : Exception
    {
        public CommandException(string Message)
            : base(Message)
        {

        }
    }
}