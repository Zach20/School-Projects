namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaceLatLng : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Place", "Lat", c => c.Double(nullable: false));
            AddColumn("dbo.Place", "Lng", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Place", "Lng");
            DropColumn("dbo.Place", "Lat");
        }
    }
}
