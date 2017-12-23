namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipSP : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Relationship_Select",
                p => new
                {
                    userID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM[dbo].[Relationship]
                    WHERE ([UserOneID] = @userID) OR ([UserTwoID] = @userID)"
            );
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Relationship_Select");
        }
    }
}
