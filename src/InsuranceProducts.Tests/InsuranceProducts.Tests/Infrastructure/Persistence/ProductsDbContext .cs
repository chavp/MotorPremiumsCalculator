using InsuranceProducts.Tests.Domain.Products.Entities;
using InsuranceProducts.Tests.Domain.SharedKernel;
using InsuranceProducts.Tests.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace InsuranceProducts.Tests.Infrastructure.Persistence
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        // Products
        public DbSet<Product> Products { get; set; }

        // Coverage availability
        public DbSet<CoverageAvailabilityType> CoverageAvailabilityTypes { get; set; }
        public DbSet<CoverageAvailability> CoverageAvailabilities { get; set; }

        // Coverages
        public DbSet<CoverageType> CoverageTypes { get; set; }
        public DbSet<CoverageLevelType> CoverageLevelTypes { get; set; }
        public DbSet<CoverageBasis> CoverageBasises { get; set; }
        public DbSet<CoverageLevel> CoverageLevels { get; set; }

        // Units
        public DbSet<UnitCategory> UnitCategories { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations for all entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);

            //modelBuilder.ApplyConfiguration(new ProductConfiguration());

            //modelBuilder.ApplyConfiguration(new CoverageAvailabilityTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new CoverageAvailabilityConfiguration());

            //modelBuilder.ApplyConfiguration(new CoverageTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new CoverageLevelConfiguration());
            //modelBuilder.ApplyConfiguration(new CoverageLevelTypeConfiguration());
            //modelBuilder.ApplyConfiguration(new CoverageBasisConfiguration());

            //// Unit Of Measure
            //modelBuilder.ApplyConfiguration(new UnitCategoryConfiguration());
            //modelBuilder.ApplyConfiguration(new UnitConfiguration());

            // Configure schema separation for bounded contexts
            ConfigureSchemas(modelBuilder);

            // Configure global conventions
            ConfigureConventions(modelBuilder);
        }

        private static void ConfigureSchemas(ModelBuilder modelBuilder)
        {
            var productsSchemas = "products";

            // Products schema
            modelBuilder.Entity<Product>().ToTable("Products", productsSchemas);

            // Coverages schema
            modelBuilder.Entity<CoverageType>().ToTable("CoverageTypes", productsSchemas);

            modelBuilder.Entity<CoverageLevelType>().ToTable("CoverageLevelTypes", productsSchemas);
            modelBuilder.Entity<CoverageBasis>().ToTable("CoverageBasises", productsSchemas);
            modelBuilder.Entity<CoverageLevel>().ToTable("CoverageLevels", productsSchemas);
            modelBuilder.Entity<CoverageAmount>().ToTable("CoverageAmounts", productsSchemas);

            // Product Coverage Availabilities schema
            modelBuilder.Entity<CoverageAvailabilityType>().ToTable("CoverageAvailabilityTypes", productsSchemas);
            modelBuilder.Entity<CoverageAvailability>().ToTable("CoverageAvailabilities", productsSchemas);

            // Unit Of Measure
            modelBuilder.Entity<UnitCategory>().ToTable("UnitCategories", productsSchemas);
            modelBuilder.Entity<Unit>().ToTable("Units", productsSchemas);
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
