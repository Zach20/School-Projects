namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlaceInsert : DbMigration
    {
        public override void Up()
        {
            DropStoredProcedure("dbo.Place_Insert");
            CreateStoredProcedure(
                "dbo.Place_Insert",
                p => new
                {
                    PlaceID = p.String(maxLength: 128),
                    Name = p.String(),
                    Lat = p.Double(),
                    Lng = p.Double()
                },
                body:
                    @"INSERT [dbo].[Place]([PlaceID], [Name], [Lat], [Lng])
	                VALUES (@PlaceID, @Name, @Lat, @Lng)"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Place_Insert");
        }
    }
}
