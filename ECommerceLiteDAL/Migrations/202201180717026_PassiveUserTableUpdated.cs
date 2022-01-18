namespace ECommerceLiteDAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassiveUserTableUpdated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PassiveUsers", "TargetRole", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PassiveUsers", "TargetRole");
        }
    }
}
