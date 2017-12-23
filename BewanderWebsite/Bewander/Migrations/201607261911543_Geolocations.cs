namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Geolocations : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .ForeignKey("dbo.State", t => t.StateID)
                .Index(t => t.StateID)
                .Index(t => t.CountryID);
            
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
                "dbo.State",
                c => new
                    {
                        StateID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Lat = c.Single(nullable: false),
                        Lng = c.Single(nullable: false),
                        CountryID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.StateID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .Index(t => t.CountryID);
            
            AddColumn("dbo.Place", "CityID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "StateID", c => c.String(maxLength: 128));
            AddColumn("dbo.Place", "CountryID", c => c.String(maxLength: 128));
            AddColumn("dbo.Review", "CityID", c => c.String(maxLength: 128));
            AddColumn("dbo.Review", "StateID", c => c.String(maxLength: 128));
            AddColumn("dbo.Review", "CountryID", c => c.String(maxLength: 128));
            AlterColumn("dbo.Place", "Name", c => c.String());
            AlterColumn("dbo.Place", "Lat", c => c.Single(nullable: false));
            AlterColumn("dbo.Place", "Lng", c => c.Single(nullable: false));
            AlterColumn("dbo.Review", "PlaceID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Place", "CityID");
            CreateIndex("dbo.Place", "StateID");
            CreateIndex("dbo.Place", "CountryID");
            CreateIndex("dbo.Review", "PlaceID");
            CreateIndex("dbo.Review", "CityID");
            CreateIndex("dbo.Review", "StateID");
            CreateIndex("dbo.Review", "CountryID");
            AddForeignKey("dbo.Place", "CityID", "dbo.City", "CityID");
            AddForeignKey("dbo.Place", "CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.Review", "CityID", "dbo.City", "CityID");
            AddForeignKey("dbo.Review", "CountryID", "dbo.Country", "CountryID");
            //AddForeignKey("dbo.Review", "PlaceID", "dbo.Place", "PlaceID");
            AddForeignKey("dbo.Place", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.Review", "StateID", "dbo.State", "StateID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Review", "StateID", "dbo.State");
            DropForeignKey("dbo.Place", "StateID", "dbo.State");
            DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            //DropForeignKey("dbo.Review", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.Review", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Review", "CityID", "dbo.City");
            DropForeignKey("dbo.Place", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "CityID", "dbo.City");
            DropForeignKey("dbo.City", "CountryID", "dbo.Country");
            DropIndex("dbo.State", new[] { "CountryID" });
            DropIndex("dbo.Review", new[] { "CountryID" });
            DropIndex("dbo.Review", new[] { "StateID" });
            DropIndex("dbo.Review", new[] { "CityID" });
            DropIndex("dbo.Review", new[] { "PlaceID" });
            DropIndex("dbo.Place", new[] { "CountryID" });
            DropIndex("dbo.Place", new[] { "StateID" });
            DropIndex("dbo.Place", new[] { "CityID" });
            DropIndex("dbo.City", new[] { "CountryID" });
            DropIndex("dbo.City", new[] { "StateID" });
            AlterColumn("dbo.Review", "PlaceID", c => c.String(nullable: false));
            AlterColumn("dbo.Place", "Lng", c => c.Single());
            AlterColumn("dbo.Place", "Lat", c => c.Single());
            AlterColumn("dbo.Place", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Review", "CountryID");
            DropColumn("dbo.Review", "StateID");
            DropColumn("dbo.Review", "CityID");
            DropColumn("dbo.Place", "CountryID");
            DropColumn("dbo.Place", "StateID");
            DropColumn("dbo.Place", "CityID");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.City");
        }
    }
}
