namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewPlace : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Place",
                c => new
                    {
                        PlaceID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        StreetNumber = c.Int(),
                        Route = c.String(),
                        PostalCode = c.String(),
                        CityID = c.Int(),
                        StateID = c.Int(),
                        CountryID = c.Int(),
                        Lat = c.Single(),
                        Lng = c.Single(),
                        Website = c.String(),
                    })
                .PrimaryKey(t => t.PlaceID)
                .ForeignKey("dbo.Country", t => t.CountryID)
                .ForeignKey("dbo.State", t => t.StateID)
                .ForeignKey("dbo.City", t => t.CityID)
                .Index(t => t.CityID)
                .Index(t => t.StateID)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StateID = c.Int(),
                        CountryID = c.Int(),
                        Country_CountryID = c.Int(),
                        Country_CountryID1 = c.Int(),
                        State_CountryID = c.Int(),
                    })
                .PrimaryKey(t => t.CityID)
                .ForeignKey("dbo.Country", t => t.Country_CountryID)
                .ForeignKey("dbo.State", t => t.StateID)
                .ForeignKey("dbo.Country", t => t.Country_CountryID1)
                .ForeignKey("dbo.Country", t => t.State_CountryID)
                .Index(t => t.StateID)
                .Index(t => t.Country_CountryID)
                .Index(t => t.Country_CountryID1)
                .Index(t => t.State_CountryID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        StateID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StateID)
                .ForeignKey("dbo.Country", t => t.CountryID, cascadeDelete: true)
                .Index(t => t.CountryID);
            
            CreateTable(
                "dbo.Review",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        PlaceID = c.String(nullable: false),
                        UserID = c.String(maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 50),
                        Body = c.String(nullable: false, maxLength: 700),
                        Subject = c.Int(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        ResidentType = c.Int(nullable: false),
                        StarRating = c.Int(),
                        CostRating = c.Int(),
                        Flag = c.Int(),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            AddColumn("dbo.File", "PlaceID", c => c.String(maxLength: 128));
            CreateIndex("dbo.File", "PlaceID");
            AddForeignKey("dbo.File", "PlaceID", "dbo.Place", "PlaceID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Review", "UserID", "dbo.User");
            DropForeignKey("dbo.File", "PlaceID", "dbo.Place");
            DropForeignKey("dbo.City", "State_CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "CityID", "dbo.City");
            DropForeignKey("dbo.City", "Country_CountryID1", "dbo.Country");
            DropForeignKey("dbo.Place", "StateID", "dbo.State");
            DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            DropForeignKey("dbo.Place", "CountryID", "dbo.Country");
            DropForeignKey("dbo.City", "Country_CountryID", "dbo.Country");
            DropIndex("dbo.Review", new[] { "UserID" });
            DropIndex("dbo.State", new[] { "CountryID" });
            DropIndex("dbo.City", new[] { "State_CountryID" });
            DropIndex("dbo.City", new[] { "Country_CountryID1" });
            DropIndex("dbo.City", new[] { "Country_CountryID" });
            DropIndex("dbo.City", new[] { "StateID" });
            DropIndex("dbo.Place", new[] { "CountryID" });
            DropIndex("dbo.Place", new[] { "StateID" });
            DropIndex("dbo.Place", new[] { "CityID" });
            DropIndex("dbo.File", new[] { "PlaceID" });
            DropColumn("dbo.File", "PlaceID");
            DropTable("dbo.Review");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.City");
            DropTable("dbo.Place");
        }
    }
}
