namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    public partial class ImageLike : DbMigration
 
    
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImageLike",
                c => new
                    {
                        ImageLikeID = c.Int(nullable: false, identity: true),
                        UserID = c.String(),
                        FileID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImageLikeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImageLike");
        }
    }
}
