namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedEntityTicket : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ticket", "ZipCode", c => c.String(maxLength: 8));
            AddColumn("dbo.Ticket", "Street", c => c.String(maxLength: 300));
            AddColumn("dbo.Ticket", "AddressNumber", c => c.String(maxLength: 10));
            AddColumn("dbo.Ticket", "City", c => c.String(maxLength: 100));
            AddColumn("dbo.Ticket", "Country", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ticket", "Country");
            DropColumn("dbo.Ticket", "City");
            DropColumn("dbo.Ticket", "AddressNumber");
            DropColumn("dbo.Ticket", "Street");
            DropColumn("dbo.Ticket", "ZipCode");
        }
    }
}
