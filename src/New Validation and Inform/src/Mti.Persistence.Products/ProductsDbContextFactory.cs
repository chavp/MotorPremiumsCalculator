using Microsoft.EntityFrameworkCore;

namespace Mti.Persistence.Products;

public class ProductsDbContextFactory : IDbContextFactory<ProductsDbContext>
{
    private DbContextOptions<ProductsDbContext> _options;

    public ProductsDbContextFactory(string connectionString)
    {
        _options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseNpgsql(connectionString)
            .Options;
    }

    public ProductsDbContext CreateDbContext()
    {
        return new ProductsDbContext(_options);
    }
}
