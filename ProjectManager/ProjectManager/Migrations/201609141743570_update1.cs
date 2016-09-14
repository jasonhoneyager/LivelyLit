namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProjectModels", name: "projectPaymentMethod", newName: "projectPaymentMethodID");
            RenameColumn(table: "dbo.ProjectModels", name: "projectPaymentType", newName: "projectPaymentTypeID");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_projectPaymentType", newName: "IX_projectPaymentTypeID");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_projectPaymentMethod", newName: "IX_projectPaymentMethodID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProjectModels", name: "IX_projectPaymentMethodID", newName: "IX_projectPaymentMethod");
            RenameIndex(table: "dbo.ProjectModels", name: "IX_projectPaymentTypeID", newName: "IX_projectPaymentType");
            RenameColumn(table: "dbo.ProjectModels", name: "projectPaymentTypeID", newName: "projectPaymentType");
            RenameColumn(table: "dbo.ProjectModels", name: "projectPaymentMethodID", newName: "projectPaymentMethod");
        }
    }
}
