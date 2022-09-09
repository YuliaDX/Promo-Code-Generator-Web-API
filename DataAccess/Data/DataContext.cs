using Core;
using Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class DataContext:DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DataContext( DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<PromoCode>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<CustomerPreference>(entity =>
            {
                entity.HasKey(e => new {e.CustomerId, e.PreferenceId });
                
            });
            modelBuilder.Entity<CustomerPreference>()
               .HasOne(bc => bc.Customer)
               .WithMany(b => b.Preferences)
               .HasForeignKey(bc => bc.CustomerId);
            modelBuilder.Entity<CustomerPreference>()
                .HasOne(bc => bc.Preference)
                .WithMany()
                .HasForeignKey(bc => bc.PreferenceId);


        }
    }
}
