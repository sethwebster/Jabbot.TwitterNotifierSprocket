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
    }
}
