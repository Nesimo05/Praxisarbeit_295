using Microsoft.EntityFrameworkCore;
using Ski_Service_Backend.Model;
using System.Security.Cryptography;

namespace Ski_Service_Backend.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<RegistrationUser> Registrations { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, UserName = "admin", Password = "admin123" },
                new User() { Id = 2, UserName = "benutzer1", Password = "B1" },
                new User() { Id = 3, UserName = "Benutzer2", Password = "B2" },
                new User() { Id = 4, UserName = "Benutzer3", Password = "B3" },
                new User() { Id = 5, UserName = "Benutzer4", Password = "B4" },
                new User() { Id = 6, UserName = "Benutzer5", Password = "B5" },
                new User() { Id = 7, UserName = "Benutzer6", Password = "B6" },
                new User() { Id = 8, UserName = "Benutzer7", Password = "B7" },
                new User() { Id = 9, UserName = "Benutzer8", Password = "B8" },
                new User() { Id = 10, UserName = "Benutzer9", Password = "B9" },
                new User() { Id = 11, UserName = "Benutzer10", Password = "B10" }
            );

            // Seed Priorities
            modelBuilder.Entity<Priority>().HasData(
                new Priority() { PriorityID = 1, PriorityType = "Tief", DaysToCompletion = 12 },
                new Priority() { PriorityID = 2, PriorityType = "Standard", DaysToCompletion = 7 },
                new Priority() { PriorityID = 3, PriorityType = "Express", DaysToCompletion = 5 }
            );



        }
    }
}