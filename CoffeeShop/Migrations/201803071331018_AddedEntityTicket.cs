namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEntityTicket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ticket",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 150),
                        Name = c.String(maxLength: 200),
                        Email = c.String(nullable: false, maxLength: 100),
                        PaypalReference = c.String(),
                        CoffeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Coffee", t => t.CoffeeId)
                .Index(t => t.CoffeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ticket", "CoffeeId", "dbo.Coffee");
            DropIndex("dbo.Ticket", new[] { "CoffeeId" });
            DropTable("dbo.Ticket");
        }
    }
}
