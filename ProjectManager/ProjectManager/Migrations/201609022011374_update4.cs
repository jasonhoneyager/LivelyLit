namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectStatusModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        projectStatusName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectStatusModels");
        }
    }
}
