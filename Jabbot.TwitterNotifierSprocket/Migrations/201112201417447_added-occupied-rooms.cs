namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class addedoccupiedrooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "OccupiedRooms",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        UserNameRequesting = c.String(),
                        DateJoined = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("OccupiedRooms");
        }
    }
}
