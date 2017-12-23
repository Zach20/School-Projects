namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isreadNotificationsFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "isread", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "isread");
        }
    }
}
