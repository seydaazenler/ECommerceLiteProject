namespace ECommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryTableUpdated : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Categories", new[] { "TopCategoryId" });
            RenameColumn(table: "dbo.Categories", name: "TopCategoryId", newName: "BaseCategoryId");
            AlterColumn("dbo.Categories", "BaseCategoryId", c => c.Int());
            CreateIndex("dbo.Categories", "BaseCategoryId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Categories", new[] { "BaseCategoryId" });
            AlterColumn("dbo.Categories", "BaseCategoryId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Categories", name: "BaseCategoryId", newName: "TopCategoryId");
            CreateIndex("dbo.Categories", "TopCategoryId");
        }
    }
}
