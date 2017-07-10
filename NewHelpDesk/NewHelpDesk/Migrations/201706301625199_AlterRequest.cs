namespace NewHelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterRequest : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Requests", name: "User_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Requests", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Requests", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "User_Id");
        }
    }
}
