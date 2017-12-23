namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserProfileSP : DbMigration
    {
        public override void Up()
        {

            CreateStoredProcedure(
                "dbo.UserProfile_Select",
                p => new
                {
                    UserID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT *
                    FROM [dbo].[UserProfile]
                    WHERE ([UserID] = @UserID)"
            );

            CreateStoredProcedure(
                "dbo.UserProfile_Insert",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        HomeTown = p.String(),
                        About = p.String(),
                        DOB = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[UserProfile]([UserID], [HomeTown], [About], [DOB])
                      VALUES (@UserID, @HomeTown, @About, @DOB)"
            );
            
            CreateStoredProcedure(
                "dbo.UserProfile_Update",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        HomeTown = p.String(),
                        About = p.String(),
                        DOB = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[UserProfile]
                      SET [HomeTown] = @HomeTown, [About] = @About, [DOB] = @DOB
                      WHERE ([UserID] = @UserID)"
            );
            
            CreateStoredProcedure(
                "dbo.UserProfile_Delete",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[UserProfile]
                      WHERE ([UserID] = @UserID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.UserProfile_Delete");
            DropStoredProcedure("dbo.UserProfile_Update");
            DropStoredProcedure("dbo.UserProfile_Insert");
        }
    }
}
