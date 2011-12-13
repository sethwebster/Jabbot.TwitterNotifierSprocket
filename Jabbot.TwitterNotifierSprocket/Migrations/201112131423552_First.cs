namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        JabbrUserName = c.String(maxLength: 4000),
                        TwitterUserName = c.String(maxLength: 4000),
                        LastNotification = c.DateTime(nullable: false),
                        EnableNotifications = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "EdmMetadata",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelHash = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("EdmMetadata");
            DropTable("Users");
        }
    }
}
