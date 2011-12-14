using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jabbot;
using Jabbot.Models;
using Jabbot.TwitterNotifierSprocket.Models;
using System.Reflection;

namespace Jabbot.TwitterNotifierSprocket
{
    public class CommandManager
    {
        private Bot _bot;
        private ChatMessage _message;
        private ITwitterNotifierSprocketRepository _database;
        private string[] _args;
        private string _command;

        public static bool _isDisabled = false;
        public CommandManager(ChatMessage message, Bot bot, ITwitterNotifierSprocketRepository database)
        {
            string[] args = message.Content
                 .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            _command = args.Length > 1 ? args[1] : string.Empty;
            _args = args.Skip(2).ToArray();

            this._bot = bot;
            this._database = database;
            this._message = message;
        }

        public bool HandleCommand()
        {
            if (_message.Content.StartsWith("twitterbot?") || _message.Content.StartsWith("@twitterbot?") ||
                _message.Content.StartsWith("@twitterbot"))
            {
                if (!HandleAdminCommand())
                {
                    if (!_isDisabled)
                    {
                        if (!HandleUserCommand())
                        {
                            _bot.PrivateReply(_message.FromUser, "Try twitterbot help for commands");
                        }
                    }
                    else
                    {
                        _bot.PrivateReply(_message.FromUser, "Twitter_bot is currently disabled.");
                    }
                }
                return true;
            }
            return false;
        }

        private bool HandleAdminCommand()
        {
            if (_command.Equals("shutdown", StringComparison.OrdinalIgnoreCase))
            {
                return HandleShutDown();
            }
            if (_command.Equals("startup", StringComparison.OrdinalIgnoreCase))
            {
                return HandleStartUp();
            }
            if (_command.Equals("listusers", StringComparison.OrdinalIgnoreCase))
            {
                return HandleListUsers();
            }
            if (_command.Equals("twitteruserfor", StringComparison.OrdinalIgnoreCase))
            {
                return HandleTwitterUserFor();
            }

            return false;
        }

        private bool HandleUserCommand()
        {
            if (string.IsNullOrEmpty(_command) ||
                       _command.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                return HandleHelp();
            }
            if (_command.Equals("twittername", StringComparison.OrdinalIgnoreCase))
            {
                return HandleTwitterName();
            }
            if (_command.Equals("on", StringComparison.OrdinalIgnoreCase))
            {
                return HandleOn();
            }
            if (_command.Equals("off", StringComparison.OrdinalIgnoreCase))
            {
                return HandleOff();
            }
            if (_command.Equals("join", StringComparison.OrdinalIgnoreCase))
            {
                return HandleJoin();
            }
            if (_command.Equals("disableinvite", StringComparison.OrdinalIgnoreCase))
            {
                return HandleDisableInvite();
            }
            return false;
        }

        private bool HandleDisableInvite()
        {
            var user = _database.FetchOrCreateUser(_message.FromUser);
            user.DisableInvites = true;
            _database.SaveChanges();
            _bot.PrivateReply(_message.FromUser, "OK - I'll leave you alone.");
            return true;
        }

        private bool HandleTwitterUserFor()
        {
            if (_message.FromUser == "sethwebster")
            {
                if (_args.Length == 2)
                {
                    string forUserName = _args[0];
                    string twitterUserName = _args[1];
                    var userFor = _database.FetchOrCreateUser(forUserName);
                    if (userFor != null)
                    {
                        userFor.TwitterUserName = twitterUserName;
                        _database.SaveChanges();
                        _bot.PrivateReply(_message.FromUser, String.Format("{0}'s twitter user name is now {1}", userFor.JabbrUserName, userFor.TwitterUserName));
                    }
                    else
                    {
                        _bot.PrivateReply(_message.FromUser, String.Format("User {0} was not found", _args[0]));
                    }
                }
                else
                {
                    _bot.PrivateReply(_message.FromUser, "twitteruserfor [user] [twitterscreenname]");
                }
            }
            else
            {
                _bot.Reply(_message.FromUser, "You're not the boss of me!", _message.Room);
            }
            return true;
        }

        private bool HandleShutDown()
        {
            if (_message.FromUser == "sethwebster")
            {
                _isDisabled = true;
                _bot.PrivateReply(_message.FromUser, "I have been disabled.");
            }
            else
            {
                _bot.Reply(_message.FromUser, "You're not the boss of me!", _message.Room);
            }
            return true;
        }

        private bool HandleStartUp()
        {
            if (_message.FromUser == "sethwebster")
            {
                _isDisabled = false;

                _bot.PrivateReply(_message.FromUser, "I have been enabled.");
            }
            else
            {
                _bot.Reply(_message.FromUser, "You're not the boss of me!", _message.Room);
            }
            return true;

        }

        private bool HandleListUsers()
        {
            if (_message.FromUser == "sethwebster")
            {
                var users = _database.Users.OrderBy(u => u.JabbrUserName).ToArray().Select(
                    u => String.Format(
                        "{0} <{1}>", u.JabbrUserName,
                        string.IsNullOrEmpty(u.TwitterUserName) ? "empty" : "@" + u.TwitterUserName)
                    );
                _bot.PrivateReply(_message.FromUser, String.Join(", ", users.ToArray()));
            }
            else
            {
                _bot.Reply(_message.FromUser, "You're not the boss of me!", _message.Room);
            }
            return true;
        }

        private bool HandleJoin()
        {
            if (_args.Length == 0)
            {
                _bot.PrivateReply(_message.FromUser, "You must specify a room to join");
            }
            else
            {
                try
                {
                    _bot.Join(_args[0]);
                    _bot.PrivateReply(_message.FromUser, "OK - I'm now in " + _args[0]);
                }
                catch (Exception e)
                {
                    _bot.PrivateReply(_message.FromUser, e.GetBaseException().Message);
                }
            }
            return true;
        }

        private bool HandleOn()
        {
            var user = _database.FetchOrCreateUser(_message.FromUser);
            user.EnableNotifications = true;
            _database.SaveChanges();
            _bot.PrivateReply(_message.FromUser, "Twitter notifications have been enabled");
            return true;
        }

        private bool HandleOff()
        {
            var user = _database.FetchOrCreateUser(_message.FromUser);
            user.EnableNotifications = false;
            _database.SaveChanges();
            _bot.PrivateReply(_message.FromUser, "Twitter notifications have been disabled");
            return true;
        }

        private bool HandleHelp()
        {
            _bot.PrivateReply(_message.FromUser, "Jabbot Twitter Sprocket - " + Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
            _bot.PrivateReply(_message.FromUser, string.Format("Status: {0}", _isDisabled ? "Disabled" : "Enabled"));
            _bot.PrivateReply(_message.FromUser, Properties.Resources.HelpText);
            return true;
        }

        private bool HandleTwitterName()
        {
            if (_args.Length == 0)
            {
                var user = _database.FetchOrCreateUser(_message.FromUser);
                _bot.PrivateReply(_message.FromUser, String.Format("Your Twitter user is {0}",
                    String.IsNullOrEmpty(user.TwitterUserName) ? "<empty>" : user.TwitterUserName));
            }
            else
            {
                _database.SetTwitterUserName(_message.FromUser, _args[0]);
                _bot.PrivateReply(_message.FromUser, String.Format("Your Twitter user name is now {0}", _args[0]));
            }
            return true;
        }
    }
}
