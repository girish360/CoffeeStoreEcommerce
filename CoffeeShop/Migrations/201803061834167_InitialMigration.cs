namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coffee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        ImageUrl = c.String(maxLength: 300),
                        Price = c.Int(nullable: false),
                        IsTheBest = c.Boolean(nullable: false),
                        LongDescription = c.String(maxLength: 1700),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Coffee");
        }
    }
}
