namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class removenewfield : DbMigration
    {
        public override void Up()
        {
            DropColumn("Users", "NewField");
        }
        
        public override void Down()
        {
            AddColumn("Users", "NewField", c => c.String());
        }
    }
}
