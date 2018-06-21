using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebProjekat.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(): base()
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Car> Cars{ get; set; }
        public virtual DbSet<Ride> Rides{ get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ride>()
                .HasOptional<Customer>(ride => ride.Customer)
                .WithMany(customer => customer.Rides)
                .HasForeignKey(ride => ride.CustomerId);

            modelBuilder.Entity<Ride>()
                .HasOptional<Dispatcher>(ride => ride.Dispatcher)
                .WithMany(dispatcher => dispatcher.Rides)
                .HasForeignKey(ride => ride.DispatcherId);

            modelBuilder.Entity<Ride>()
                .HasOptional<Driver>(ride => ride.Driver)
                .WithMany(driver => driver.Rides)
                .HasForeignKey(ride => ride.DriverId);

            modelBuilder.Entity<Comment>()
                .HasRequired<Ride>(comment => comment.Ride)
                .WithOptional(ride => ride.Comment);

            modelBuilder.Entity<Car>()
                .HasOptional<Driver>(car => car.Driver)
                .WithOptionalDependent(driver => driver.Car);

            modelBuilder.Entity<Driver>()
                .HasOptional<Car>(driver => driver.Car)
                .WithOptionalPrincipal(car => car.Driver);



        }

        public System.Data.Entity.DbSet<WebProjekat.Models.Customer> Customers { get; set; }
    }
}