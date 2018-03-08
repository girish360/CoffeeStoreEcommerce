namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEcommerceEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Order", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Int(nullable: false),
                        Tax = c.Int(nullable: false),
                        SubTotal = c.Int(nullable: false),
                        Shipping = c.Int(nullable: false),
                        PaypalReference = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItem", "OrderId", "dbo.Order");
            DropIndex("dbo.OrderItem", new[] { "OrderId" });
            DropTable("dbo.Order");
            DropTable("dbo.OrderItem");
        }
    }
}
