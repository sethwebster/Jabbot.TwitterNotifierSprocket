using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public interface ITwitterNotifierSprocketRepository
    {
        IDbSet<User> Users { get; set; }
        int SaveChanges();
    }
}
