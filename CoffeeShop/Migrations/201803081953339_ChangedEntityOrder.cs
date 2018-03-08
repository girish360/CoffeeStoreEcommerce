namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntityOrder : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Order", newName: "Orders");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Orders", newName: "Order");
        }
    }
}
