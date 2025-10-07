using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence
{
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
}
