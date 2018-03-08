namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ticket", "Name", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ticket", "Name", c => c.String(maxLength: 200));
        }
    }
}
