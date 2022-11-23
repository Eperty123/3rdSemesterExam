using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class DatabaseContext : DbContext
    {
        DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            //Booking
            modelBuilder.Entity<Booking>()
                .Property(b => b.Id) 
                .ValueGeneratedOnAdd();

            //Set up foreign keys
            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.Coach)
                .WithMany(coach => coach.Bookings)
                .HasForeignKey(booking => booking.Coach.Id)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.Client)
                .WithMany(client => client.Bookings)
                .HasForeignKey(booking => booking.Client.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Booking> BookingTable { get; set; }
    }
}
