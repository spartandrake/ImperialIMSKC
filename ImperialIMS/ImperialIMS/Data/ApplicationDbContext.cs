using ImperialIMS.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ImperialIMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private IConfiguration _config;
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Manifest> Manifests { get; set; }
        public DbSet<Shipment> Shipmetns { get; set; }
        public DbSet<StorageFacility> StorageFacilities { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Need a configuration object
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(
                _config.GetConnectionString("DefaultConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create identity models
            base.OnModelCreating(modelBuilder);
            // Check for concurrency of entities in modelBuilder
            modelBuilder.Entity<Alert>().Property(a => a.TimeStamp).IsConcurrencyToken();
            
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var entry in entries) entry.Property("TimeStamp").CurrentValue = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {

                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    bool identicalValues = true;
                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];
                        if (!proposedValue.Equals(databaseValue))
                        {
                            identicalValues = false;
                            break;
                        }
                    }
                    if (identicalValues)
                    {
                        return base.SaveChanges();
                    }
                    entry.OriginalValues.SetValues(databaseValues);

                }
                throw;
            }
        }
    }
}
