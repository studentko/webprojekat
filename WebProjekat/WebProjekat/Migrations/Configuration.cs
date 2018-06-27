namespace WebProjekat.Migrations
{
    using Models;
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

            context.Users.AddOrUpdate(
               new Dispatcher { Id = 1, Username = "admin1", Password = "123", Gender = Gender.Female, Role = Role.Dispatcher },
               new Dispatcher { Id = 2, Username = "admin2", Password = "123", Gender = Gender.Female, Role = Role.Dispatcher },
               new Dispatcher { Id = 3, Username = "admin3", Password = "123", Gender = Gender.Female, Role = Role.Dispatcher }
            );

            context.Cars.AddOrUpdate(
                new Car { Id = 1, CarType = CarType.Car, ModelYear = 1996, RegistrationNumber = "NS 1000", DriverId = 4 },
                new Car { Id = 2, CarType = CarType.Car, ModelYear = 1996, RegistrationNumber = "NS 1001", DriverId = 5 },
                new Car { Id = 3, CarType = CarType.Car, ModelYear = 1996, RegistrationNumber = "NS 1002", DriverId = 6 }
                );

            context.Users.AddOrUpdate(
                new Driver { Id = 4, Username = "driver1", Password = "123", FirstName = "Driver", LastName = "Prvi", Role = Role.Driver, CarId = 1, Location = new Location()},
                new Driver { Id = 5, Username = "driver2", Password = "123", FirstName = "Driver", LastName = "Drugi", Role = Role.Driver, CarId = 2, Location = new Location()},
                new Driver { Id = 6, Username = "driver3", Password = "123", FirstName = "Driver", LastName = "Treci", Role = Role.Driver, CarId = 3, Location = new Location()}
                );

            

            context.Users.AddOrUpdate(
                new Customer { Id = 7, Username = "user1", Password = "123", Role = Role.Customer },
                new Customer { Id = 8, Username = "user2", Password = "123", Role = Role.Customer },
                new Customer { Id = 9, Username = "user3", Password = "123", Role = Role.Customer }
                );
        }

    }
}
