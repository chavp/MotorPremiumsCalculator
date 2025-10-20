using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mti.Domain.Products.Entities;
using Mti.Domain.SharedKernel.Primatives;

namespace Mti.Persistence.Products;

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
    public DbSet<CoverageTypeComposition> CoverageTypeCompositions { get; set; }

    // Units
    public DbSet<UnitCategory> UnitCategories { get; set; }
    public DbSet<Unit> Units { get; set; }

    // Products Features
    public DbSet<ProductFeatureType> ProductFeatureTypes { get; set; }
    public DbSet<ProductFeature> ProductFeatures { get; set; }
    public DbSet<VehicleFuelType> VehicleFuelTypes { get; set; }
    public DbSet<VehicleUsage> VehicleUsages { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    public DbSet<VehicleSize> VehicleSizes { get; set; }
    public DbSet<VehicleTypeCompulsory> VehicleTypeCompulsories { get; set; }
    public DbSet<VehicleTypeVoluntary> VehicleTypeVoluntaries { get; set; }
    public DbSet<VehicleWorkshopType> VehicleWorkshopTypes { get; set; }

    // Product feature availability
    public DbSet<ProductFeatureAvailabilityType> ProductFeatureAvailabilityTypes { get; set; }
    public DbSet<ProductFeatureAvailability> ProductFeatureAvailabilities { get; set; }

    // Products
    public DbSet<PolicyType> PolicyTypes { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }

    // Premiums
    public DbSet<PeriodType> PeriodTypes { get; set; }
    public DbSet<InsuranceRate> InsuranceRates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations for all entities
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);

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
        modelBuilder.Entity<CoverageTypeComposition>().ToTable("CoverageTypeCompositions", productsSchemas);

        modelBuilder.Entity<CoverageLevelType>().ToTable("CoverageLevelTypes", productsSchemas);
        modelBuilder.Entity<CoverageBasis>().ToTable("CoverageBasises", productsSchemas);
        modelBuilder.Entity<CoverageLevel>().ToTable("CoverageLevels", productsSchemas);
        modelBuilder.Entity<CoverageAmount>().ToTable("CoverageAmounts", productsSchemas);
        modelBuilder.Entity<CoverageRange>().ToTable("CoverageRanges", productsSchemas);
        modelBuilder.Entity<CoverageLimit>().ToTable("CoverageLimits", productsSchemas);

        // Product Coverage Availabilities schema
        modelBuilder.Entity<CoverageAvailabilityType>().ToTable("CoverageAvailabilityTypes", productsSchemas);
        modelBuilder.Entity<CoverageAvailability>().ToTable("CoverageAvailabilities", productsSchemas);

        // Unit Of Measure
        modelBuilder.Entity<UnitCategory>().ToTable("UnitCategories", productsSchemas);
        modelBuilder.Entity<Unit>().ToTable("Units", productsSchemas);

        // Product Features
        modelBuilder.Entity<ProductFeatureType>().ToTable("ProductFeatureTypes", productsSchemas);
        modelBuilder.Entity<ProductFeature>().ToTable("ProductFeatures", productsSchemas);
        modelBuilder.Entity<VehicleVoluntaryFeature>().ToTable("VehicleVoluntaryFeatures", productsSchemas);
        modelBuilder.Entity<VehicleCompulsoryFeature>().ToTable("VehicleCompulsoryFeatures", productsSchemas);
        modelBuilder.Entity<VehiclePriceGroupFeature>().ToTable("VehiclePriceGroupFeatures", productsSchemas);
        modelBuilder.Entity<VehicleBrandFeature>().ToTable("VehicleBrandFeatures", productsSchemas);
        modelBuilder.Entity<VehicleModelFeature>().ToTable("VehicleModelFeatures", productsSchemas);
        modelBuilder.Entity<VehicleFuelType>().ToTable("VehicleFuelTypes", productsSchemas);
        modelBuilder.Entity<VehicleUsage>().ToTable("VehicleUsages", productsSchemas);
        modelBuilder.Entity<VehicleType>().ToTable("VehicleTypes", productsSchemas);
        modelBuilder.Entity<VehicleSize>().ToTable("VehicleSizes", productsSchemas);
        modelBuilder.Entity<VehicleTypeCompulsory>().ToTable("VehicleTypeCompulsories", productsSchemas);
        modelBuilder.Entity<VehicleTypeVoluntary>().ToTable("VehicleTypeVoluntaries", productsSchemas);
        modelBuilder.Entity<VehicleWorkshopType>().ToTable("VehicleWorkshopTypes", productsSchemas);

        modelBuilder.Entity<ProductFeatureAvailabilityType>().ToTable("ProductFeatureAvailabilityTypes", productsSchemas);
        modelBuilder.Entity<ProductFeatureAvailability>().ToTable("ProductFeatureAvailabilities", productsSchemas);

        // Products
        modelBuilder.Entity<PolicyType>().ToTable("PolicyTypes", productsSchemas);
        modelBuilder.Entity<Campaign>().ToTable("Campaigns", productsSchemas);

        modelBuilder.Entity<Campaign>()
        .HasMany(e => e.Products)
        .WithMany(e => e.Campaigns)
        .UsingEntity(
            "CampaignProducts",
            r => r.HasOne(typeof(Product)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(Product.Id)),
            l => l.HasOne(typeof(Campaign)).WithMany().HasForeignKey("CampaignId").HasPrincipalKey(nameof(Campaign.Id)),
            j => j.HasKey("CampaignId", "ProductId"));

        // Premiums
        modelBuilder.Entity<PeriodType>().ToTable("PeriodTypes", productsSchemas);
        modelBuilder.Entity<InsuranceRate>().ToTable("InsuranceRates", productsSchemas);
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
        updateAuditableEntities(DateTime.UtcNow);

        return base.SaveChanges();
    }

    private void updateAuditableEntities(DateTime utcNow)
    {
        foreach (EntityEntry<IAuditableEntity> entityEntry in ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedDateUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.LastModifiedDateUtc)).CurrentValue = utcNow;

                var revCurrentValue = entityEntry.Property(nameof(Entity<IAuditableEntity>.Revision)).CurrentValue;
                entityEntry.Property(nameof(Entity<IAuditableEntity>.Revision)).CurrentValue = Convert.ToUInt64(revCurrentValue) + 1;
            }
        }
    }
}
