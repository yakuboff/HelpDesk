namespace NewHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRequestForeignKey : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Requests", "ApplicationUserId");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "ApplicationUserId");
            AlterColumn("dbo.Requests", "Status", c => c.Int());
            AlterColumn("dbo.Requests", "Priority", c => c.Int());
            AlterColumn("dbo.Requests", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "ApplicationUserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Requests", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Requests", "ApplicationUserId", c => c.Int());
            AlterColumn("dbo.Requests", "Priority", c => c.Int(nullable: false));
            AlterColumn("dbo.Requests", "Status", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Requests", name: "ApplicationUserId", newName: "ApplicationUser_Id");
            AddColumn("dbo.Requests", "ApplicationUserId", c => c.Int());
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
        }
    }
}
