using Microsoft.EntityFrameworkCore;
using JobPortalApi.Database.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace JobPortalApi.Database
{
    public class DatabaseContext : IdentityDbContext<User, Role, Guid>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<ReservationLine> ReservationLines { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedDatabase(builder);
        }

        private static void SeedDatabase(ModelBuilder builder)
        {
            var userIdOne = Guid.NewGuid();
            var userIdTwo = Guid.NewGuid();
            var roleIdOne = Guid.NewGuid();
            var roleIdTwo = Guid.NewGuid();

            builder.Entity<User>().ToTable("Users");

            // Roles
            var roles = new List<Role>
            {
                new() {Id = roleIdOne, Name = "Admin", NormalizedName = "ADMIN"},
                new() {Id = roleIdTwo, Name = "User", NormalizedName = "USER"}
            };

            builder.Entity<Role>().HasData(roles);

            // Users
            var users = new List<User>
            {
                new()
                {
                    Id = userIdOne,
                    FullName = "Test user second",
                    UserName = "user@test.com",
                    NormalizedUserName = "USER@TEST.COM",
                    Email = "user@test.com",
                    NormalizedEmail = "USER@TEST.COM",
                },

                new()
                {
                    Id = userIdTwo,
                    FullName = "Test user",
                    UserName = "user2@test.com",
                    NormalizedUserName = "USER2@TEST.COM",
                    Email = "user2@test.com",
                    NormalizedEmail = "USER2@TEST.COM",
                },
            };

            builder.Entity<User>().HasData(users);

            var passwordHasher = new PasswordHasher<User>();
            users[0].PasswordHash = passwordHasher.HashPassword(users[0], "string");
            users[1].PasswordHash = passwordHasher.HashPassword(users[1], "string");

            // userRoles
            var userRoles = new List<UserRole>
            {
                new()
                {
                    UserId = userIdOne,
                    RoleId = roleIdOne
                },
                new()
                {
                    UserId = userIdTwo,
                    RoleId = roleIdTwo
                }
            };
            builder.Entity<UserRole>().HasData(userRoles);
        }
    }
}