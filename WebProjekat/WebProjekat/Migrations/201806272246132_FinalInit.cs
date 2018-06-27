namespace WebProjekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalInit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriverId = c.Int(nullable: false),
                        ModelYear = c.Int(nullable: false),
                        RegistrationNumber = c.String(),
                        CarType = c.Int(nullable: false),
                        Driver_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Driver_Id)
                .Index(t => t.Driver_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Gender = c.Int(nullable: false),
                        JMBG = c.String(),
                        PhoneNumber = c.String(),
                        Role = c.Int(nullable: false),
                        Blocked = c.Boolean(nullable: false),
                        CarId = c.Int(),
                        Location_X = c.Double(),
                        Location_Y = c.Double(),
                        Location_StreetName = c.String(),
                        Location_StreetNumber = c.Int(),
                        Location_AreaCode = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rides",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(),
                        DriverId = c.Int(),
                        DispatcherId = c.Int(),
                        Date = c.DateTime(nullable: false),
                        StartLocation_X = c.Double(nullable: false),
                        StartLocation_Y = c.Double(nullable: false),
                        StartLocation_StreetName = c.String(),
                        StartLocation_StreetNumber = c.Int(nullable: false),
                        StartLocation_AreaCode = c.String(),
                        Destination_X = c.Double(nullable: false),
                        Destination_Y = c.Double(nullable: false),
                        Destination_StreetName = c.String(),
                        Destination_StreetNumber = c.Int(nullable: false),
                        Destination_AreaCode = c.String(),
                        Amount = c.Int(nullable: false),
                        CarType = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.DispatcherId)
                .ForeignKey("dbo.Users", t => t.DriverId)
                .Index(t => t.CustomerId)
                .Index(t => t.DriverId)
                .Index(t => t.DispatcherId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        RideId = c.Int(nullable: false),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Rides", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "Driver_Id", "dbo.Users");
            DropForeignKey("dbo.Rides", "DriverId", "dbo.Users");
            DropForeignKey("dbo.Rides", "DispatcherId", "dbo.Users");
            DropForeignKey("dbo.Rides", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.Comments", "Id", "dbo.Rides");
            DropForeignKey("dbo.Comments", "CustomerId", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "CustomerId" });
            DropIndex("dbo.Comments", new[] { "Id" });
            DropIndex("dbo.Rides", new[] { "DispatcherId" });
            DropIndex("dbo.Rides", new[] { "DriverId" });
            DropIndex("dbo.Rides", new[] { "CustomerId" });
            DropIndex("dbo.Cars", new[] { "Driver_Id" });
            DropTable("dbo.Comments");
            DropTable("dbo.Rides");
            DropTable("dbo.Users");
            DropTable("dbo.Cars");
        }
    }
}
