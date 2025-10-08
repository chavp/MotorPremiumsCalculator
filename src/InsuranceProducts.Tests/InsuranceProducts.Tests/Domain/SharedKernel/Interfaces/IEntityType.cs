using InsuranceProducts.Tests.Domain.Products.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.SharedKernel.Interfaces
{
    public interface IEntityType
    {
        public Code Code { get; protected set; }
        public Description Description { get; protected set; }

        public void UpdateDescription(Description description);
    }
}
