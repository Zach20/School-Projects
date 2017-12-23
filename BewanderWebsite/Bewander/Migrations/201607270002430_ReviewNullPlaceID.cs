namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewNullPlaceID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropIndex("dbo.Review", new[] { "PlaceID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Review", "PlaceID");
            AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropIndex("dbo.Review", new[] { "PlaceID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Review", "PlaceID");
            AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID", cascadeDelete: true);
        }
    }
}
