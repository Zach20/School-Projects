namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFlaggedImages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.File", "flagged", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.File", "flagged");
        }
    }
}
