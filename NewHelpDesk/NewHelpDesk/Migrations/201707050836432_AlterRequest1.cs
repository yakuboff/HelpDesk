namespace NewHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRequest1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "ExecutorId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "StatusId");
            CreateIndex("dbo.Requests", "PriorityId");
            CreateIndex("dbo.Requests", "ExecutorId");
            AddForeignKey("dbo.Requests", "ExecutorId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Requests", "PriorityId", "dbo.Priorities", "Id");
            AddForeignKey("dbo.Requests", "StatusId", "dbo.Status", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Requests", "PriorityId", "dbo.Priorities");
            DropForeignKey("dbo.Requests", "ExecutorId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ExecutorId" });
            DropIndex("dbo.Requests", new[] { "PriorityId" });
            DropIndex("dbo.Requests", new[] { "StatusId" });
            DropColumn("dbo.Requests", "ExecutorId");
        }
    }
}
