namespace ECommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdutCodeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductCode", c => c.String(maxLength: 8));
            CreateIndex("dbo.Products", "ProductCode", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "ProductCode" });
            DropColumn("dbo.Products", "ProductCode");
        }
    }
}
