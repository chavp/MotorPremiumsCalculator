using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Infrastructure.Persistence
{
    // docker run --name insurance-db -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=admin123 -d -p 5432:5432 postgres
    public class ProductsDbDesignTimeContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=insurance-db;Username=admin;Password=admin123");

            // Enable sensitive data logging in development
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();

            return new ProductsDbContext(optionsBuilder.Options);
        }
    }
}
