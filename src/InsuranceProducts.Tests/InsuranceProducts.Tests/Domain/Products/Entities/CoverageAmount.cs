using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.Products.Entities
{
    public sealed class CoverageAmount : CoverageLevel
    {
        public decimal Amount { get; private set; }

        protected CoverageAmount() { }

        public CoverageAmount SetAmount(decimal amount) 
        {
            Amount = amount;
            return this;
        }

        protected internal CoverageAmount(Guid id,
            CoverageLevelType type,
            CoverageBasis basis,
            Unit unit) : base(id, type, basis, unit)
        {
            
        }
    }
}
