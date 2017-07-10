namespace NewHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Priorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Requests", "StatusId", c => c.Int());
            AddColumn("dbo.Requests", "PriorityId", c => c.Int());
            DropColumn("dbo.Requests", "Status");
            DropColumn("dbo.Requests", "Priority");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "Priority", c => c.Int());
            AddColumn("dbo.Requests", "Status", c => c.Int());
            DropColumn("dbo.Requests", "PriorityId");
            DropColumn("dbo.Requests", "StatusId");
            DropTable("dbo.Status");
            DropTable("dbo.Priorities");
        }
    }
}
