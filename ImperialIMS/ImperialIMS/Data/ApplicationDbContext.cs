using ImperialIMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImperialIMS.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Manifest> Manifests { get; set; }
        public DbSet<Shipment> Shipmetns { get; set; }
        public DbSet<StorageFacility> StorageFacilities { get; set; }
    }
}
