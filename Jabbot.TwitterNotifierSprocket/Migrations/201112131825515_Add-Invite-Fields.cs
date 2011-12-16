namespace Jabbot.TwitterNotifierSprocket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddInviteFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("Users", "LastInviteDate", c => c.DateTime(nullable: false, defaultValue: DateTime.Now));
            AddColumn("Users", "DisableInvites", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("Users", "DisableInvites");
            DropColumn("Users", "LastInviteDate");
        }
    }
}
