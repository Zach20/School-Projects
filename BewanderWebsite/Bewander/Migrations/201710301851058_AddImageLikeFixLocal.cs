namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageLikeFixLocal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminUserViewModel",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        LockOutEnabled = c.Boolean(nullable: false),
                        LockOutEndDate = c.DateTime(nullable: false),
                        Role = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        DateJoined = c.DateTime(nullable: false),
                        DaysAsMember = c.Int(nullable: false),
                        HomeTownID = c.String(),
                        FavoritePlaceID = c.String(),
                        HomeTown = c.String(),
                        HomeTownName = c.String(),
                        FavoritePlace = c.String(),
                        FavoritePlaceName = c.String(),
                        PostFlag = c.Int(nullable: false),
                        ReviewFlag = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AdminUserViewModel");
        }
    }
}
