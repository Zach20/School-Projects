namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserSP : DbMigration
    {
        public override void Up()
        {
            //DropStoredProcedure("dbo.User_Delete");
            //DropStoredProcedure("dbo.User_Update");
            //DropStoredProcedure("dbo.User_Insert");
            //DropStoredProcedure("dbo.User_Select");

            CreateStoredProcedure(
                "dbo.User_Select",
                p => new
                {
                    UserID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT *
                    FROM [dbo].[User]
                    WHERE ([UserID] = @UserID)"
            );

            CreateStoredProcedure(
                "dbo.User_Insert",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Permission = p.Int(),
                        DateJoined = p.DateTime(),
                        ConnectionID = p.String(),
                        Connected = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[User]([UserID], [FirstName], [LastName], [Permission], [DateJoined], [ConnectionID], [Connected])
                      VALUES (@UserID, @FirstName, @LastName, @Permission, @DateJoined, @ConnectionID, @Connected)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Update",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                        FirstName = p.String(),
                        LastName = p.String(),
                        Permission = p.Int(),
                        DateJoined = p.DateTime(),
                        ConnectionID = p.String(),
                        Connected = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[User]
                      SET [FirstName] = @FirstName, [LastName] = @LastName, [Permission] = @Permission, [DateJoined] = @DateJoined, [ConnectionID] = @ConnectionID, [Connected] = @Connected
                      WHERE ([UserID] = @UserID)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Delete",
                p => new
                    {
                        UserID = p.String(maxLength: 128),
                    },
                body:
                    @"DELETE [dbo].[User]
                      WHERE ([UserID] = @UserID)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.User_Delete");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Insert");
            DropStoredProcedure("dbo.User_Select");
        }
    }
}
