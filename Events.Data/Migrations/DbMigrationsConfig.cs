namespace Events.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public  sealed class DbMigrationsConfig : DbMigrationsConfiguration<Events.Data.ApplicationDbContext>
    {
        public DbMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Events.Data.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

           /* if (!context.Users.Any())
            {
                var adminEmail = "admin@domain.com";
                var adminUserName = adminEmail;
                var adminFullName = "App Administrator";
                var adminPassword = adminEmail + "C78";
                string adminRole = "Administrator";

                CreateAdminUser(context, adminEmail, adminUserName, adminFullName, adminPassword, adminRole);
                CreateSeveralEvents(context);

            }*/
        }

        public void RunSeed(Events.Data.ApplicationDbContext db)
        {
            Seed(db);
        }

        private void CreateSeveralEvents(ApplicationDbContext context)
        {
            context.Events.Add(new Event()
            {
                Title = "Ye ye lol",
                StartDateTime = DateTime.Now.Date.AddDays(3).AddHours(15).AddMinutes(10),
                Author = context.Users.First(),
            });

            context.Events.Add(new Event()
            {
                Title = "Passed Event <Anonymous>",
                StartDateTime = DateTime.Now.Date.AddDays(-2.5).AddHours(5).AddMinutes(2),
                Duration = TimeSpan.FromHours(.5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() { Text = "<Anonymous> comment"},
                    new Comment() {
                        Text = "User comment",
                        Author = context.Users.First()
                    }
                }
            });

            context.Events.Add(new Event()
            {
                Title = "KLK Event <Anonymous>",
                StartDateTime = DateTime.Now.Date.AddDays(15).AddHours(25).AddMinutes(15),
                Duration = TimeSpan.FromHours(5),
                Comments = new HashSet<Comment>()
                {
                    new Comment() { Text = "<Anonymous> comment"},
                    new Comment() {
                        Text = "User comment",
                        Author = context.Users.First()
                    }
                }
            });
        }
        /// <summary>
        /// Creates admin user account
        /// </summary>
        /// <param name="context"></param>
        /// <param name="adminEmail"></param>
        /// <param name="adminUserName"></param>
        /// <param name="adminFullName"></param>
        /// <param name="adminPassword"></param>
        /// <param name="adminRole"></param>
        private void CreateAdminUser(ApplicationDbContext context, string adminEmail, string adminUserName, string adminFullName, string adminPassword, string adminRole)
        {
            //Create "admin" user
            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                FullName = adminFullName,
                Email = adminEmail
            };

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore)
            {
                PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true
                }
            };

            var userCreateResult = userManager.Create(adminUser, adminPassword);
            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }

            //Create "Administrator" Role
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var roleCreateResult = roleManager.Create(new IdentityRole(adminRole));
            if (!roleCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", roleCreateResult.Errors));
            }

            //Add "admin" user to "Administrator" role
            var addAdminRoleResult = userManager.AddToRole(adminUser.Id, adminRole);
            if (!addAdminRoleResult.Succeeded)
            {
                throw new Exception(string.Join("; ", addAdminRoleResult.Errors));
            }
        }
    }
}
