using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopHub.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopHub.Model.Context
{
    public class ShopHubContext : DbContext
    {
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ShopHubContext() : base(GetOptions()) { }    //Inherit the options from the parent
        public ShopHubContext(DbContextOptions<ShopHubContext> options) //takes config of our ShopHubContext class
            : base(options) { }

        private static DbContextOptions GetOptions()    //The configuration for the DB Context Options
        {
            var builder = new ConfigurationBuilder();   //We are building the config for the middleware
            builder.AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
                //Populating connection string to the SQL Server
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), configuration.GetConnectionString("ShopHubConnection")).Options;
            //options.UseSqlServer("Server= DESKTOP-KGITG3T; Database=Store; Trusted_connection = true;");

        }
    }
}
