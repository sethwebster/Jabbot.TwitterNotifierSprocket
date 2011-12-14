using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public class TwitterNotifierSprocketRepository : DbContext, ITwitterNotifierSprocketRepository
    {
        public virtual IDbSet<User> Users { get; set; }

        public void RecordActivity(string fromUser)
        {
            var user = FetchOrCreateUser(fromUser);
            user.LastActivity = DateTime.Now;
            this.SaveChanges();
        }

        public bool SetTwitterUserName(string forUser, string twitterUserName)
        {
            var user = FetchOrCreateUser(forUser);
            user.TwitterUserName = twitterUserName;
            this.SaveChanges();
            return true;
        }

        public User FetchOrCreateUser(string forUser)
        {
            var user = this.Users.FirstOrDefault(u => u.JabbrUserName == forUser);
            if (user == null)
            {
                user = new User()
                {
                    LastNotification = DateTime.Now.AddMinutes(-60),
                    JabbrUserName = forUser
                };
                this.Users.Add(user);
                this.SaveChanges();
            }
            return user;
        }

        public void MarkUserNotified(string UserName)
        {
            var userRec = this.Users.FirstOrDefault(u => u.JabbrUserName == UserName);
            if (userRec == null)
            {
                userRec = new User()
                {
                    JabbrUserName = UserName
                };
                this.Users.Add(userRec);
            }
            userRec.LastNotification = DateTime.Now;
            this.SaveChanges();
        }

    }
}
