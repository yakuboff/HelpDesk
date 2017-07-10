namespace NewHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestAndCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Comment = c.String(),
                        Status = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        ApplicationUserId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Requests", new[] { "User_Id" });
            DropIndex("dbo.Requests", new[] { "CategoryId" });
            DropTable("dbo.Requests");
            DropTable("dbo.Categories");
        }
    }
}
