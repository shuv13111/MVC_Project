using Microsoft.EntityFrameworkCore;
using ShoeTrackr.Models;
using System.Text.RegularExpressions;

namespace ShoeTrackr.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
           
        }
        public  DbSet<ItemModel> items { get; set; }

        public DbSet<PurchaseModel> purchases { get; set; }

        public DbSet<SaleItemModel> saleItemModels { get; set; }

        public DbSet<SalesModel> sales { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemModelConfiguration());
            modelBuilder.ApplyConfiguration(new SaleItemModelConfiguration());

            modelBuilder.ApplyConfiguration(new PurchaseModelConfiguration());
           

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=SURUKK;Database=ShoeTracker;Trusted_Connection=True;TrustServerCertificate=True");
        }

    }
}
