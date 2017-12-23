namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class City1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.City", name: "Country_CountryID", newName: "CountryID");
            RenameColumn(table: "dbo.City", name: "State_StateID", newName: "StateID");
            RenameIndex(table: "dbo.City", name: "IX_State_StateID", newName: "IX_StateID");
            RenameIndex(table: "dbo.City", name: "IX_Country_CountryID", newName: "IX_CountryID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.City", name: "IX_CountryID", newName: "IX_Country_CountryID");
            RenameIndex(table: "dbo.City", name: "IX_StateID", newName: "IX_State_StateID");
            RenameColumn(table: "dbo.City", name: "StateID", newName: "State_StateID");
            RenameColumn(table: "dbo.City", name: "CountryID", newName: "Country_CountryID");
        }
    }
}
