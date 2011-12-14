using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public class User
    {
        public User()
        {
            EnableNotifications = true;
            LastActivity = DateTime.Now.AddMinutes(-60);
            LastInviteDate = DateTime.Now.AddDays(-1);
        }
        [Key]
        public int UserId { get; set; }
        public string JabbrUserName { get; set; }
        public string TwitterUserName { get; set; }
        public DateTime LastNotification { get; set; }
        public bool EnableNotifications { get; set; }
        public DateTime LastActivity { get; set; }
        public DateTime LastInviteDate { get; set; }
        public bool DisableInvites { get; set; }

        public override string ToString()
        {
            return string.Format("{0} <{1}>", JabbrUserName, TwitterUserName);
        }
    }
}
