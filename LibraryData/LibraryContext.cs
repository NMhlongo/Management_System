using System.Collections.Generic;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryData
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CompanyAsset> CompanyAssets { get; set; }
        public DbSet<RentCard> RentCards { get; set; }
        public DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public DbSet<Checkout> Checkouts  { get; set; }
        public DbSet<Hold> Holds { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}
