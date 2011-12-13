namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class newfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "NewField", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Users", "NewField");
        }
    }
}
