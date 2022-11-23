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
            
        }

        public DbSet<User> UserTable { get; set; }
        public DbSet<Booking> BookingTable { get; set; }
        
    }
}
