namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class StoredProcedures : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Check_Friends",
                p => new
                {
                    currentUserID = p.String(maxLength: 128),
                    userID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM[dbo].[Relationship]
                    WHERE ([UserOneID] = @currentUserID OR[UserTwoID] = @currentUserID) AND([UserOneID] = @userID OR[UserTwoID] = @userID)"
            );

            CreateStoredProcedure(
                "dbo.File_Search",
                p => new
                {
                    userID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM [dbo].[File]
	                WHERE [UserID] = @userID"
            );

            CreateStoredProcedure(
                "dbo.Place_Insert",
                p => new
                {
                    PlaceID = p.String(maxLength: 128),
                    Name = p.String()
                },
                body:
                    @"INSERT [dbo].[Place]([PlaceID], [Name])
	                VALUES (@PlaceID, @Name)"
            );

            CreateStoredProcedure(
                "dbo.Place_Search",
                p => new
                {
                    term = p.String()
                },
                body:
                    @"SELECT TOP 5 *
	                FROM [dbo].[Place]
	                WHERE Name LIKE '%' + @term + '%' 
                    ORDER BY Name ASC"
            );

            CreateStoredProcedure(
                "dbo.Place_Select",
                p => new
                {
                    PlaceID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT *
	                FROM [dbo].[Place]
	                Where ([PlaceID] = @PlaceID)"
                );

            CreateStoredProcedure(
                "dbo.Post_Search",
                p => new
                {
                    userID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM [dbo].[Post]
	                WHERE [UserID] = @userID"
            );

            CreateStoredProcedure(
                "dbo.Review_Insert",
                p => new
                {
                    PlaceID = p.String(maxLength: 128),
                    UserID = p.String(maxLength: 128),
                    Title = p.String(),
                    Body = p.String(),
                    Subject = p.Int(),
                    DatePosted = p.DateTime(),
                    ResidentType = p.Int(),
                    StarRating = p.Int(),
                    CostRating = p.Int(),
                    Flag = p.Int(),
                    ImageID = p.Int()
                },
                body:
                    @"INSERT [dbo].[Review]([PlaceID], [UserID], [Title], [Body], [Subject], [DatePosted], [ResidentType], [StarRating], [CostRating], [Flag], [ImageID])
	                VALUES (@PlaceID, @UserID, @Title, @Body, @Subject, @DatePosted, @ResidentType, @StarRating, @CostRating, @Flag, @ImageID)"
            );

            CreateStoredProcedure(
                "dbo.Review_Search",
                p => new
                {
                    userID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM [dbo].[Review]
	                WHERE [UserID] = @userID"
            );

            CreateStoredProcedure(
                "dbo.Review_Select",
                p => new
                {
                    ReviewID = p.String(maxLength: 128)
                },
                body:
                    @"SELECT * FROM [dbo].[Review]
	                Where [ReviewID] = @ReviewID"
            );

            CreateStoredProcedure(
                "dbo.User_Search",
                p => new
                {
                    term1 = p.String(maxLength: 128),
                    term2 = p.String(maxLength: 128)
                },
                body:
                    @"SELECT TOP 5 *
	                FROM [dbo].[User]
	                WHERE FirstName LIKE '%' + @term1 + '%'
	                AND LastName LIKE '%' + @term2 + '%'
	                ORDER BY FirstName ASC"
            );

            CreateStoredProcedure(
                "dbo.User_Search_One",
                p => new
                {
                    term = p.String(maxLength: 128)
                },
                body:
                    @"SELECT TOP 5 *
	                FROM [dbo].[User]
	                WHERE FirstName LIKE '%' + @term + '%'
	                OR LastName LIKE '%' + @term + '%'
	                ORDER BY FirstName ASC"
            );
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.Check_Friends");
            DropStoredProcedure("dbo.File_Search");
            DropStoredProcedure("dbo.Place_Insert");
            DropStoredProcedure("dbo.Place_Search");
            DropStoredProcedure("dbo.Place_Select");
            DropStoredProcedure("dbo.Post_Search");
            DropStoredProcedure("dbo.Review_Insert");
            DropStoredProcedure("dbo.Review_Search");
            DropStoredProcedure("dbo.Review_Select");
            DropStoredProcedure("dbo.User_Search");
            DropStoredProcedure("dbo.User_Search_One");
        }
    }
}