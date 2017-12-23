namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PastLocalETC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "PastLocal", c => c.String());
            AddColumn("dbo.UserProfile", "LastTraveled", c => c.String());
            AddColumn("dbo.UserProfile", "FavoritePlace", c => c.String());
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
                    },
                body:
                    @"INSERT [dbo].[UserProfile]([UserID], [HomeTown], [PastLocal], [LastTraveled], [FavoritePlace], [About], [DOB])
                      VALUES (@UserID, @HomeTown, @PastLocal, @LastTraveled, @FavoritePlace, @About, @DOB)"
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
                    },
                body:
                    @"UPDATE [dbo].[UserProfile]
                      SET [HomeTown] = @HomeTown, [PastLocal] = @PastLocal, [LastTraveled] = @LastTraveled, [FavoritePlace] = @FavoritePlace, [About] = @About, [DOB] = @DOB
                      WHERE ([UserID] = @UserID)"
            );
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "FavoritePlace");
            DropColumn("dbo.UserProfile", "LastTraveled");
            DropColumn("dbo.UserProfile", "PastLocal");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
