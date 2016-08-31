namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        projectName = c.String(),
                        projectCategory = c.String(),
                        projectDescription = c.String(),
                        projectRequestedDueDate = c.DateTime(nullable: false),
                        projectOfferedPaymentType = c.String(),
                        projectOfferedPaymentAmount = c.String(),
                        projectPaymentMethod = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectModels");
        }
    }
}
