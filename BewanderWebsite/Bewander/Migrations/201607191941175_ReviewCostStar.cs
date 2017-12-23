namespace Bewander.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReviewCostStar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Review", "StarRating", c => c.Int(nullable: false));
            AlterColumn("dbo.Review", "CostRating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Review", "CostRating", c => c.Int());
            AlterColumn("dbo.Review", "StarRating", c => c.Int());
        }
    }
}
