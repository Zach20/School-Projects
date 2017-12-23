namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewPlaceIDFK : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Review", new[] { "PlaceID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Review", "PlaceID");
            AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Review", "PlaceID");
            DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropIndex("dbo.Review", new[] { "PlaceID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(nullable: false));
        }
    }
}
