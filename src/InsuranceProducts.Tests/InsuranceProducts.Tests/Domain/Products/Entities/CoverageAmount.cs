using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageAmount : CoverageLevel
    {
        public decimal Amount { get; private set; }

        protected CoverageAmount() { }

        public CoverageAmount(Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit,
            decimal amount) : base(id, type, basis, unit)
        {
            Amount = amount;
        }
    }
}
