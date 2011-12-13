namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddActivityTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "LastActivity", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Users", "LastActivity");
        }
    }
}
