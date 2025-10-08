using EnsureThat;
using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using InsuranceProducts.Tests.Domain.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public abstract class CoverageLevel : Entity<Guid>
    {
        public CoverageLevelType? CoverageLevelType { get; protected set; }
        public CoverageBasis? CoverageBasis { get; protected set; }
        public Unit? Unit { get; protected set; }

        protected CoverageLevel() { }

        protected CoverageLevel(
            Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit) : base(id)
        {
            Id = Ensure.That(id).NotEmpty("Id is not empty", nameof(id));
            CoverageLevelType = type;
            CoverageBasis = basis;
            Unit = unit;
        }
    }
}
