namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Level = c.Int(nullable: false),
                        Flag = c.Int(nullable: false),
                        DatePosted = c.DateTime(nullable: false),
                        PostID = c.Int(nullable: false),
                        Comment_ID = c.Int(),
                        User_UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.Comment_ID)
                .ForeignKey("dbo.Post", t => t.PostID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.User_UserID)
                .Index(t => t.PostID)
                .Index(t => t.Comment_ID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Like",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        Comment_ID = c.Int(),
                        Post_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Comment", t => t.Comment_ID)
                .ForeignKey("dbo.Post", t => t.Post_ID)
                .Index(t => t.Comment_ID)
                .Index(t => t.Post_ID);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Caption = c.String(),
                        PhotoID = c.Int(),
                        DatePosted = c.DateTime(nullable: false),
                        Flag = c.Int(nullable: false),
                        UserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Permission = c.Int(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        ConnectionID = c.String(),
                        Connected = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Path = c.String(maxLength: 255),
                        FileType = c.Int(nullable: false),
                        Caption = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                        UserID = c.String(maxLength: 128),
                        DateProfilePic = c.DateTime(),
                        DateCoverPhoto = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Relationship",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserOneID = c.String(),
                        UserTwoID = c.String(),
                        Status = c.Int(nullable: false),
                        UserOneOnline = c.Boolean(nullable: false),
                        UserTwoOnline = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserID = c.String(nullable: false, maxLength: 128),
                        HomeTown = c.String(),
                        About = c.String(),
                        DOB = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "User_UserID", "dbo.User");
            DropForeignKey("dbo.Post", "UserID", "dbo.User");
            DropForeignKey("dbo.File", "UserID", "dbo.User");
            DropForeignKey("dbo.Like", "Post_ID", "dbo.Post");
            DropForeignKey("dbo.Comment", "PostID", "dbo.Post");
            DropForeignKey("dbo.Like", "Comment_ID", "dbo.Comment");
            DropForeignKey("dbo.Comment", "Comment_ID", "dbo.Comment");
            DropIndex("dbo.File", new[] { "UserID" });
            DropIndex("dbo.Post", new[] { "UserID" });
            DropIndex("dbo.Like", new[] { "Post_ID" });
            DropIndex("dbo.Like", new[] { "Comment_ID" });
            DropIndex("dbo.Comment", new[] { "User_UserID" });
            DropIndex("dbo.Comment", new[] { "Comment_ID" });
            DropIndex("dbo.Comment", new[] { "PostID" });
            DropTable("dbo.UserProfile");
            DropTable("dbo.Relationship");
            DropTable("dbo.File");
            DropTable("dbo.User");
            DropTable("dbo.Post");
            DropTable("dbo.Like");
            DropTable("dbo.Comment");
        }
    }
}
