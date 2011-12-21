using System.Data.Entity;

namespace Jabbot.TwitterNotifierSprocket.Models
{
    public interface ITwitterNotifierSprocketRepository
    {
        IDbSet<User> Users { get; set; }
        IDbSet<OccupiedRoom> OccupiedRooms { get; set; }
        int SaveChanges();
        void RecordActivity(string fromUser);
        User FetchOrCreateUser(string forUser);
        bool SetTwitterUserName(string forUser, string twitterUserName);
        void JoinRoom(string roomName, string requestingUser);
        void MarkUserNotified(string UserName);

    }
}
