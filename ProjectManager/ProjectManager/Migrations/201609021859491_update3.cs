namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategoryModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        categoryName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.ProjectModels", "projectCategory");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProjectModels", "projectCategory", c => c.String());
            DropTable("dbo.CategoryModels");
        }
    }
}
