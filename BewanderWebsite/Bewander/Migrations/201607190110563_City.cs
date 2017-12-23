namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class City : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.City", "Country_CountryID1", "dbo.Country");
            DropForeignKey("dbo.City", "State_CountryID", "dbo.Country");
            DropForeignKey("dbo.File", "PlaceID", "dbo.Place");
            DropIndex("dbo.City", new[] { "Country_CountryID1" });
            DropIndex("dbo.City", new[] { "State_CountryID" });
            RenameColumn(table: "dbo.City", name: "StateID", newName: "State_StateID");
            RenameIndex(table: "dbo.City", name: "IX_StateID", newName: "IX_State_StateID");
            DropPrimaryKey("dbo.Place");
            AlterColumn("dbo.Place", "PlaceID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Place", "PlaceID");
            AddForeignKey("dbo.File", "PlaceID", "dbo.Place", "PlaceID");
            DropColumn("dbo.City", "CountryID");
            DropColumn("dbo.City", "Country_CountryID1");
            DropColumn("dbo.City", "State_CountryID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.City", "State_CountryID", c => c.Int());
            AddColumn("dbo.City", "Country_CountryID1", c => c.Int());
            AddColumn("dbo.City", "CountryID", c => c.Int());
            DropForeignKey("dbo.File", "PlaceID", "dbo.Place");
            DropPrimaryKey("dbo.Place");
            AlterColumn("dbo.Place", "PlaceID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Place", "PlaceID");
            RenameIndex(table: "dbo.City", name: "IX_State_StateID", newName: "IX_StateID");
            RenameColumn(table: "dbo.City", name: "State_StateID", newName: "StateID");
            CreateIndex("dbo.City", "State_CountryID");
            CreateIndex("dbo.City", "Country_CountryID1");
            AddForeignKey("dbo.File", "PlaceID", "dbo.Place", "PlaceID");
            AddForeignKey("dbo.City", "State_CountryID", "dbo.Country", "CountryID");
            AddForeignKey("dbo.City", "Country_CountryID1", "dbo.Country", "CountryID");
        }
    }
}
