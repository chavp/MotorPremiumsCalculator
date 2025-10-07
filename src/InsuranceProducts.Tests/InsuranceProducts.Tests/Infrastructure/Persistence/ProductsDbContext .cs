using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InsuranceProducts.Tests.Infrastructure.Persistence
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        // Coverages
        public DbSet<CoverageType> CoverageTypes { get; set; }
        public DbSet<CoverageLevelType> CoverageLevelTypes { get; set; }
        public DbSet<CoverageBasis> CoverageBasises { get; set; }
        public DbSet<CoverageLevel> CoverageLevels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations for all entities
            modelBuilder.ApplyConfiguration(new CoverageTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CoverageLevelTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CoverageBasisConfiguration());

            // Configure schema separation for bounded contexts
            ConfigureSchemas(modelBuilder);

            // Configure global conventions
            ConfigureConventions(modelBuilder);
        }

        private static void ConfigureSchemas(ModelBuilder modelBuilder)
        {
            // Coverages schema
            modelBuilder.Entity<CoverageType>().ToTable("CoverageTypes", "products");
            modelBuilder.Entity<CoverageLevelType>().ToTable("CoverageLevelTypes", "products");
            modelBuilder.Entity<CoverageBasis>().ToTable("CoverageBasises", "products");

            modelBuilder.Entity<CoverageLevel>().ToTable("CoverageLevels", "products");
            modelBuilder.Entity<CoverageAmount>().ToTable("CoverageAmounts", "products");
        }

        private static void ConfigureConventions(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyUpperConverter(["Code"]);

            // Configure string properties to have a reasonable default max length
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(500);
                    }
                }
            }

            // Configure DateTime properties to be stored as UTC
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(
                            new Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<DateTime, DateTime>(
                                v => v.ToUniversalTime(),
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)));
                    }
                }
            }
        }

        public override int SaveChanges()
        {
            saveChangeAsync().Wait();

            return base.SaveChanges();
        }

        private async Task saveChangeAsync(CancellationToken cancellationToken = default)
        {
            updateAuditableEntities(DateTime.UtcNow);
        }

        private void updateAuditableEntities(DateTime utcNow)
        {
            foreach (EntityEntry<IEntity> entityEntry in ChangeTracker.Entries<IEntity>())
            {
                if (entityEntry.State == EntityState.Added)
                {
                    entityEntry.Property(nameof(IEntity.CreatedDateUtc)).CurrentValue = utcNow;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Property(nameof(IEntity.LastModifiedDateUtc)).CurrentValue = utcNow;

                    //var revCurrentValue = entityEntry.Property(nameof(Entity<TId>.Revision)).CurrentValue;
                    //entityEntry.Property(nameof(Entity<TId>.Revision)).CurrentValue = Convert.ToUInt32(revCurrentValue) + 1;
                }
            }
        }
    }
}
