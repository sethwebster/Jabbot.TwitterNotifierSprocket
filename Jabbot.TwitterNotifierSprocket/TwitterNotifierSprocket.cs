using System;
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

namespace Jabbot.TwitterNotifierSprocket
{
    public class TwitterNotifierSprocket : ISprocket
    {
        private static readonly Regex _usernameMatchRegex = new Regex(@"(@\w+)");
        private static readonly string _tweetFormat = "{0}, you were just mentioned by @{1} here http://jabbr.net/#/rooms/{2}.";
        private ITwitterNotifierSprocketRepository _database;

        public TwitterNotifierSprocket()
            : this(new TwitterNotifierSprocketRepository())
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TwitterNotifierSprocketRepository>());
        }

        public TwitterNotifierSprocket(ITwitterNotifierSprocketRepository database)
        {
            this._database = database;
        }

        public bool Handle(ChatMessage message, Bot bot)
        {
            try
            {
                if (!HandleCommand(message, bot))
                {
                    var twitterUsers = GetUserNamesFromMessage(message.Content);
                    if (twitterUsers.Count() > 0)
                    {
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
                }
            }
            catch (CommandException ce)
            {
                bot.PrivateReply(message.FromUser, ce.Message);
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }

        private bool HandleCommand(ChatMessage message, Bot bot)
        {
            string[] args = message.Content
                .ToLower()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (args.Length > 0 && args[0] == "twitterbot?")
            {
                string command = args.Length > 1 ? args[1] : string.Empty;
                args = args.Skip(2).ToArray();
                if (string.IsNullOrEmpty(command) ||
                    command.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    return HandleHelp(args, bot, message);
                }
                if (command.Equals("twittername", StringComparison.OrdinalIgnoreCase))
                {
                    return HandleTwitterName(args, bot, message);
                }
                if (command.Equals("on", StringComparison.OrdinalIgnoreCase))
                {
                    return HandleOn(args, bot, message);
                }
                if (command.Equals("off", StringComparison.OrdinalIgnoreCase))
                {
                    return HandleOff(args, bot, message);
                }
                bot.PrivateReply(message.FromUser, "Try twitterbot? help for commands");
            }
            return false;
        }

        private bool HandleOn(string[] args, Bot bot, ChatMessage message)
        {
            var user = FetchOrCreateUser(message.FromUser);
            user.EnableNotifications = true;
            _database.SaveChanges();
            bot.PrivateReply(message.FromUser, "Twitter notifications have been enabled");
            return true;
        }

        private bool HandleOff(string[] args, Bot bot, ChatMessage message)
        {
            var user = FetchOrCreateUser(message.FromUser);
            user.EnableNotifications = false;
            _database.SaveChanges();
            bot.PrivateReply(message.FromUser, "Twitter notifications have been disabled");
            return true;
        }

        private bool HandleHelp(string[] args, Bot bot, ChatMessage message)
        {
            bot.PrivateReply(message.FromUser, "Jabbot Twitter Sprocket - " + Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
            bot.PrivateReply(message.FromUser, "Say:");
            bot.PrivateReply(message.FromUser, "twittername [TwitterScreenName] - Sets your Twitter user name");
            bot.PrivateReply(message.FromUser, "off - turn off Twitter notifications when you are mentioned");
            bot.PrivateReply(message.FromUser, "on - turn Twitter notifications on for when you are mentioned");
            return true;
        }

        private bool HandleTwitterName(string[] args, Bot bot, ChatMessage message)
        {
            if (args.Length == 0)
                throw new CommandException("You must supply a twitter user name");
            SetTwitterUserName(message.FromUser, args[0]);
            bot.PrivateReply(message.FromUser, String.Format("Your Twitter user name is now {0}", args[0]));
            return true;
        }

        private bool SetTwitterUserName(string forUser, string twitterUserName)
        {
            var user = FetchOrCreateUser(forUser);
            user.TwitterUserName = twitterUserName;
            _database.SaveChanges();
            return true;
        }

        private User FetchOrCreateUser(string forUser)
        {
            var user = _database.Users.FirstOrDefault(u => u.JabbrUserName == forUser);
            if (user == null)
            {
                user = new User()
                {
                    LastNotification = DateTime.Now.AddMinutes(-60),
                    JabbrUserName = forUser
                };
                _database.Users.Add(user);
            }
            return user;
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
                                        ScreenName = FetchOrCreateUser(g.Value).TwitterUserName
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
            var userRec = _database.Users.FirstOrDefault(u => u.JabbrUserName == UserName);
            if (userRec == null)
            {
                userRec = new User()
                {
                    JabbrUserName = UserName
                };
                _database.Users.Add(userRec);
            }
            userRec.LastNotification = DateTime.Now;
            _database.SaveChanges();
        }

        private bool ShouldNotifyUser(string UserName)
        {
            var user = _database.Users.FirstOrDefault(u => u.JabbrUserName == UserName);
            return user == null || (DateTime.Now - user.LastNotification).TotalMinutes >= 60 && user.EnableNotifications;
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