namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectModels", "projectDenial", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectModels", "projectDenial");
        }
    }
}
