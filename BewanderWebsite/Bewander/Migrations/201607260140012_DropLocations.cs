namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropLocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.City", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "CityID", "dbo.City");
            DropForeignKey("dbo.Place", "CountryID", "dbo.Country");
            DropForeignKey("dbo.City", "StateID", "dbo.State");
            DropForeignKey("dbo.State", "CountryID", "dbo.Country");
            DropForeignKey("dbo.Place", "StateID", "dbo.State");
            DropIndex("dbo.City", new[] { "StateID" });
            DropIndex("dbo.City", new[] { "CountryID" });
            DropIndex("dbo.Place", new[] { "CityID" });
            DropIndex("dbo.Place", new[] { "StateID" });
            DropIndex("dbo.Place", new[] { "CountryID" });
            DropIndex("dbo.State", new[] { "CountryID" });
            DropColumn("dbo.Place", "CityID");
            DropColumn("dbo.Place", "StateID");
            DropColumn("dbo.Place", "CountryID");
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
                        StateID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CountryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StateID);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StateID = c.Int(),
                        CountryID = c.Int(),
                    })
                .PrimaryKey(t => t.CityID);
            
            AddColumn("dbo.Place", "CountryID", c => c.Int());
            AddColumn("dbo.Place", "StateID", c => c.Int());
            AddColumn("dbo.Place", "CityID", c => c.Int());
            CreateIndex("dbo.State", "CountryID");
            CreateIndex("dbo.Place", "CountryID");
            CreateIndex("dbo.Place", "StateID");
            CreateIndex("dbo.Place", "CityID");
            CreateIndex("dbo.City", "CountryID");
            CreateIndex("dbo.City", "StateID");
            AddForeignKey("dbo.Place", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.State", "CountryID", "dbo.Country", "CountryID", cascadeDelete: true);
            AddForeignKey("dbo.City", "StateID", "dbo.State", "StateID");
            AddForeignKey("dbo.Place", "CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.Place", "CityID", "dbo.City", "CityID");
            AddForeignKey("dbo.City", "CountryID", "dbo.Country", "CountryID");
        }
    }
}
