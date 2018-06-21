namespace WebProjekat.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebProjekat.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebProjekat.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            /*context.Users.AddOrUpdate(
               new Dispatcher { Username = "admin", Password = "123", Gender = Gender.Female, Role = Role.Dispatcher },
               new Dispatcher { Username = "disp", Password = "321", Gender = Gender.Female, Role = Role.Dispatcher }
            );*/
        }
    }
}
