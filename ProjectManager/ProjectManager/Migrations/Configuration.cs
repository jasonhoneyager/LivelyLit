namespace ProjectManager.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

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

            context.PaymentTypeModels.AddOrUpdate(p => p.projectPaymentType,
                new PaymentTypeModels { projectPaymentType = "Per Word" },
                new PaymentTypeModels { projectPaymentType = "Up Front" }
                );

            context.PaymentMethodModels.AddOrUpdate(p => p.projectPaymentMethod,
                new PaymentMethodModels { projectPaymentMethod = "PayPal" },
                new PaymentMethodModels { projectPaymentMethod = "Check" },
                new PaymentMethodModels { projectPaymentMethod = "Escrow/Upfront" }
                );

            if (!context.Roles.Any(r => r.Name == "Writer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Writer" };

                manager.Create(role);
            }

            if (!context.Users.Any(u => u.UserName == "writer@writer.com"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "writer@writer.com" };

                manager.Create(user, password: "writerpass1");
                manager.AddToRole(user.Id, "Writer");
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
}
