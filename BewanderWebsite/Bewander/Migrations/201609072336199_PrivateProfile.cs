namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Private", c => c.Boolean(nullable: false));
            AlterStoredProcedure(
                "dbo.UserProfile_Insert",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        HomeTown = p.String(),
                        PastLocal = p.String(),
                        LastTraveled = p.String(),
                        FavoritePlace = p.String(),
                        About = p.String(),
                        DOB = p.DateTime(),
                        Private = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[UserProfile]([UserID], [HomeTown], [PastLocal], [LastTraveled], [FavoritePlace], [About], [DOB], [Private])
                      VALUES (@UserID, @HomeTown, @PastLocal, @LastTraveled, @FavoritePlace, @About, @DOB, @Private)"
            );
            
            AlterStoredProcedure(
                "dbo.UserProfile_Update",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        HomeTown = p.String(),
                        PastLocal = p.String(),
                        LastTraveled = p.String(),
                        FavoritePlace = p.String(),
                        About = p.String(),
                        DOB = p.DateTime(),
                        Private = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[UserProfile]
                      SET [HomeTown] = @HomeTown, [PastLocal] = @PastLocal, [LastTraveled] = @LastTraveled, [FavoritePlace] = @FavoritePlace, [About] = @About, [DOB] = @DOB, [Private] = @Private
                      WHERE ([UserID] = @UserID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Private");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
