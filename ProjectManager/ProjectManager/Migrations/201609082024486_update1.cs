namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CategoryModels", "ProjectModels_ID", "dbo.ProjectModels");
            DropIndex("dbo.CategoryModels", new[] { "ProjectModels_ID" });
            AddColumn("dbo.ProjectModels", "projectCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.ProjectModels", "projectCategoryID");
            AddForeignKey("dbo.ProjectModels", "projectCategoryID", "dbo.CategoryModels", "ID", cascadeDelete: true);
            DropColumn("dbo.CategoryModels", "ProjectModels_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CategoryModels", "ProjectModels_ID", c => c.Int());
            DropForeignKey("dbo.ProjectModels", "projectCategoryID", "dbo.CategoryModels");
            DropIndex("dbo.ProjectModels", new[] { "projectCategoryID" });
            DropColumn("dbo.ProjectModels", "projectCategoryID");
            CreateIndex("dbo.CategoryModels", "ProjectModels_ID");
            AddForeignKey("dbo.CategoryModels", "ProjectModels_ID", "dbo.ProjectModels", "ID");
        }
    }
}
