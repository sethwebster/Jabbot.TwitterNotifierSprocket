using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public class OccupiedRoom
    {
        public OccupiedRoom()
        {
            this.DateJoined = DateTime.Now;
        }
        [Key]
        public string Name { get; set; }
        public string UserNameRequesting { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
