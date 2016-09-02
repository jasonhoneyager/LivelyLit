namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectManager.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjectManager.Models.ApplicationDbContext context)
        {
            context.ProjectStatusModels.AddOrUpdate(p => p.projectStatusName,
                new Models.ProjectStatusModels { projectStatusName = "Pending Approval" },
                new Models.ProjectStatusModels { projectStatusName = "Declined" },
                new Models.ProjectStatusModels { projectStatusName = "Accepted" },
                new Models.ProjectStatusModels { projectStatusName = "Work In Progress" },
                new Models.ProjectStatusModels { projectStatusName = "Completed, Awaiting Payment" },
                new Models.ProjectStatusModels { projectStatusName = "Completed, Payment Received" }
                );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
