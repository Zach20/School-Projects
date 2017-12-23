namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationDataFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "data", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "data");
        }
    }
}
