namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        RelationshipID = c.Int(),
                        Content = c.String(),
                        UserID = c.String(),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.Relationship", t => t.RelationshipID)
                .Index(t => t.RelationshipID);
            
            AddColumn("dbo.Relationship", "UserOneDateTime", c => c.DateTime());
            AddColumn("dbo.Relationship", "UserTwoDateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Message", "RelationshipID", "dbo.Relationship");
            DropIndex("dbo.Message", new[] { "RelationshipID" });
            DropColumn("dbo.Relationship", "UserTwoDateTime");
            DropColumn("dbo.Relationship", "UserOneDateTime");
            DropTable("dbo.Message");
        }
    }
}
