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
        public DatabaseContext(DbContextOptions<DatabaseContext> opts) : base(opts)
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

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);


            //Booking
            modelBuilder.Entity<Booking>()
                .Property(b => b.Id) 
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Booking>()
                .HasKey(b => b.Id);


            //Set up foreign keys
            modelBuilder.Entity<Booking>()
                .HasOne<Client>(booking => booking.Client)
                .WithMany(client => client.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne<Coach>(booking => booking.Coach)
                .WithMany(coach => coach.Bookings)
                .HasForeignKey(b => b.CoachId)
                .OnDelete(DeleteBehavior.Cascade);


            //Ignore
            modelBuilder.Entity<Booking>()
                .Ignore(b => b.Coach);
            modelBuilder.Entity<Booking>()
                .Ignore(b => b.Client);
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Coach> CoachTable { get; set; }
        public DbSet<Client> ClientTable { get; set; }
        public DbSet<Booking> BookingTable { get; set; }
    }
}
