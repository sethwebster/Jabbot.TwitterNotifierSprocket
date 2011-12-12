using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string JabbrUserName { get; set; }
        public string TwitterUserName { get; set; }
        public DateTime LastNotification { get; set; }
        public bool EnableNotifications { get; set; }
    }
}
