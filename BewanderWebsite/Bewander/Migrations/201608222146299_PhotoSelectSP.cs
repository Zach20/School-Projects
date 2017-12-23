namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoSelectSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Photo_Select",
                p => new
                {
                    PhotoID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM [dbo].[File]
	                WHERE [ID] = @PhotoID"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.Photo_Select");
        }
    }
}
