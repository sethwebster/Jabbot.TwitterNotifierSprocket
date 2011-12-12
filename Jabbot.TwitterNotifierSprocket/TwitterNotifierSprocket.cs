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
        private static readonly string _tweetFormat = "@{0}, you were just mentioned by @{1} here http://jabbr.net/#/rooms/{2}.";
        private ITwitterNotifierSprocketRepository _database;
        private bool _isDisabled = false;
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
                    var user = FetchOrCreateUser(message.FromUser);
                    if (twitterUsers.Count() > 0)
                    {
                        foreach (var u in twitterUsers)
                        {
                            if (ShouldNotifyUser(u.ScreenName))
                            {
                                TweetSharp.TwitterService svc = new TwitterService(GetClientInfo());
                                svc.AuthenticateWith(ConfigurationManager.AppSettings["User.Token"],
                                    ConfigurationManager.AppSettings["User.TokenSecret"]);
                                svc.SendTweet(String.Format(_tweetFormat, u.ScreenName, String.IsNullOrEmpty(user.TwitterUserName) ? user.JabbrUserName : user.TwitterUserName, message.Room));
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
            if (args.Length > 0 && (args[0] == "twitterbot?" || args[0] == "@twitterbot?"))
            {
                string command = args.Length > 1 ? args[1] : string.Empty;
                args = args.Skip(2).ToArray();
                if (!HandleAdminCommand(command, args, bot, message))
                {
                    if (!_isDisabled)
                    {
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
                        if (command.Equals("join", StringComparison.OrdinalIgnoreCase))
                        {
                            return HandleJoin(args, bot, message);
                        }
                    }
                }
                else
                {
                    return true;
                }
                bot.PrivateReply(message.FromUser, "Try twitterbot? help for commands");
            }
            return false;
        }

        private bool HandleAdminCommand(string command, string[] args, Bot bot, ChatMessage message)
        {
            if (string.IsNullOrEmpty(command) ||
                   command.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return HandleHelp(args, bot, message);
            }
            if (command.Equals("shutdown", StringComparison.OrdinalIgnoreCase))
            {
                return HandleShutDown(args, bot, message);
            }
            if (command.Equals("startup", StringComparison.OrdinalIgnoreCase))
            {
                return HandleStartUp(args, bot, message);
            }
            return false;
        }

        private bool HandleShutDown(string[] args, Bot bot, ChatMessage message)
        {
            if (message.FromUser == "sethwebster")
            {
                _isDisabled = true;
                bot.PrivateReply(message.FromUser, "I have been disabled.");
            }
            else
            {
                bot.Reply(message.FromUser, "You are not the boss of me", message.Room);
            }
            return true;
        }

        private bool HandleStartUp(string[] args, Bot bot, ChatMessage message)
        {
            if (message.FromUser == "sethwebster")
            {
                _isDisabled = false;

                bot.PrivateReply(message.FromUser, "I have been enabled.");
            }
            else
            {
                bot.Reply(message.FromUser, "You are not the boss of me", message.Room);
            }
            return true;
        }

        private bool HandleJoin(string[] args, Bot bot, ChatMessage message)
        {
            if (args.Length == 0)
            {
                bot.PrivateReply(message.FromUser, "You must specify a room to join");
            }
            else
            {
                try
                {
                    bot.Join(args[0]);
                    bot.PrivateReply(message.FromUser, "OK - I'm now in " + args[0]);
                }
                catch (Exception e)
                {
                    bot.PrivateReply(message.FromUser, e.GetBaseException().Message);
                }
            }
            return true;
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
            bot.PrivateReply(message.FromUser, string.Format("Status: {0}", _isDisabled ? "Disabled" : "Enabled"));
            bot.PrivateReply(message.FromUser, "Say:");
            bot.PrivateReply(message.FromUser, "twittername [TwitterScreenName] - Displays or Sets your Twitter user name");
            bot.PrivateReply(message.FromUser, "off - turn OFF Twitter notifications when you are mentioned");
            bot.PrivateReply(message.FromUser, "on - turn ON Twitter notifications on for when you are mentioned");
            bot.PrivateReply(message.FromUser, "join [roomname] - ask me to join a room to watch for mentions");
            bot.PrivateReply(message.FromUser, "startup - start me watching for mentions (for everyone)");
            bot.PrivateReply(message.FromUser, "shutdown - stop me from watching for mentions (for everyone)");
            return true;
        }

        private bool HandleTwitterName(string[] args, Bot bot, ChatMessage message)
        {
            if (args.Length == 0)
            {
                var user = FetchOrCreateUser(message.FromUser);
                bot.PrivateReply(message.FromUser, String.Format("Your Twitter user is {0}",
                    String.IsNullOrEmpty(user.TwitterUserName) ? "<empty>" : user.TwitterUserName));
            }
            else
            {
                SetTwitterUserName(message.FromUser, args[0]);
                bot.PrivateReply(message.FromUser, String.Format("Your Twitter user name is now {0}", args[0]));
            }
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
                _database.SaveChanges();
            }
            return user;
        }

        IEnumerable<TwitterUser> GetUserNamesFromMessage(string message)
        {
            return _usernameMatchRegex.Matches(message)
                                .Cast<Match>()
                                .Select(g =>
                                    new TwitterUser()
                                    {
                                        ScreenName = FetchOrCreateUser(g.Value.Replace("@", "")).TwitterUserName
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