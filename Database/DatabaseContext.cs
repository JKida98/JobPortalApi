using Microsoft.EntityFrameworkCore;
using JobPortalApi.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace JobPortalApi.Database
{
    public class DatabaseContext : IdentityDbContext<User, UserRole, Guid>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<ReservationLine> ReservationLines { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetupDbUser(modelBuilder);
        }

        private static void SetupDbUser(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users");
        }
    }

    
}
