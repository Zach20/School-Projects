namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropGeoLocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.City", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "CityID", "dbo.City");
            DropForeignKey("dbo.Place", "CountryID", "dbo.Country");
            DropForeignKey("dbo.File", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Review", "CityID", "dbo.City");
            DropForeignKey("dbo.Review", "CountryID", "dbo.Country");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "StateID", "dbo.State");
            DropForeignKey("dbo.Review", "StateID", "dbo.State");
            DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropIndex("dbo.City", new[] { "StateID" });
            DropIndex("dbo.City", new[] { "CountryID" });
            DropIndex("dbo.Place", new[] { "CityID" });
            DropIndex("dbo.Place", new[] { "StateID" });
            DropIndex("dbo.Place", new[] { "CountryID" });
            DropIndex("dbo.File", new[] { "PlaceID" });
            DropIndex("dbo.Review", new[] { "PlaceID" });
            DropIndex("dbo.Review", new[] { "CityID" });
            DropIndex("dbo.Review", new[] { "StateID" });
            DropIndex("dbo.Review", new[] { "CountryID" });
            DropIndex("dbo.State", new[] { "CountryID" });
            AddColumn("dbo.File", "ReviewID", c => c.Int());
            AddColumn("dbo.Review", "ImageID", c => c.Int());
            AlterColumn("dbo.Review", "PlaceID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Review", "PlaceID");
            AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID", cascadeDelete: true);
            DropColumn("dbo.Place", "Lat");
            DropColumn("dbo.Place", "Lng");
            DropColumn("dbo.Place", "StreetNumber");
            DropColumn("dbo.Place", "Route");
            DropColumn("dbo.Place", "PostalCode");
            DropColumn("dbo.Place", "Website");
            DropColumn("dbo.Place", "CityID");
            DropColumn("dbo.Place", "StateID");
            DropColumn("dbo.Place", "CountryID");
            DropColumn("dbo.File", "PlaceID");
            DropColumn("dbo.Review", "CityID");
            DropColumn("dbo.Review", "StateID");
            DropColumn("dbo.Review", "CountryID");
            DropTable("dbo.City");
            DropTable("dbo.Country");
            DropTable("dbo.State");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        CountryID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        StateID = c.String(maxLength: 128),
                        CountryID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CityID);
            
            AddColumn("dbo.Review", "CountryID", c => c.String(maxLength: 128));
            AddColumn("dbo.Review", "StateID", c => c.String(maxLength: 128));
            AddColumn("dbo.Review", "CityID", c => c.String(maxLength: 128));
            AddColumn("dbo.File", "PlaceID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "CountryID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "StateID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "CityID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "Website", c => c.String());
            AddColumn("dbo.Place", "PostalCode", c => c.String());
            AddColumn("dbo.Place", "Route", c => c.String());
            AddColumn("dbo.Place", "StreetNumber", c => c.Int());
            AddColumn("dbo.Place", "Lng", c => c.Single(nullable: false));
            AddColumn("dbo.Place", "Lat", c => c.Single(nullable: false));
            DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropIndex("dbo.Review", new[] { "PlaceID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(maxLength: 128));
            DropColumn("dbo.Review", "ImageID");
            DropColumn("dbo.File", "ReviewID");
            CreateIndex("dbo.State", "CountryID");
            CreateIndex("dbo.Review", "CountryID");
            CreateIndex("dbo.Review", "StateID");
            CreateIndex("dbo.Review", "CityID");
            CreateIndex("dbo.Review", "PlaceID");
            CreateIndex("dbo.File", "PlaceID");
            CreateIndex("dbo.Place", "CountryID");
            CreateIndex("dbo.Place", "StateID");
            CreateIndex("dbo.Place", "CityID");
            CreateIndex("dbo.City", "CountryID");
            CreateIndex("dbo.City", "StateID");
            AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID");
            AddForeignKey("dbo.Review", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.Place", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.State", "CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.City", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.Review", "CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.Review", "CityID", "dbo.City", "CityID");
            AddForeignKey("dbo.File", "PlaceID", "dbo.Place", "PlaceID");
            AddForeignKey("dbo.Place", "CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.Place", "CityID", "dbo.City", "CityID");
            AddForeignKey("dbo.City", "CountryID", "dbo.Country", "CountryID");
        }
    }
}
