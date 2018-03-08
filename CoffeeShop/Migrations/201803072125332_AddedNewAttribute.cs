namespace CoffeeShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewAttribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coffee", "ShortDescription", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Coffee", "ShortDescription");
        }
    }
}
