namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDateCover : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.File", "DateCoverPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.File", "DateCoverPhoto", c => c.DateTime());
        }
    }
}
