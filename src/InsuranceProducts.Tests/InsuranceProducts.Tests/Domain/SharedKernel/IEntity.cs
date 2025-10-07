using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.SharedKernel
{
    interface IEntity
    {
        /// <summary>
        /// Gets the date and time when this item was created.
        /// </summary>
        public DateTime CreatedDateUtc { get; }

        /// <summary>
        /// Gets the date and time when this item was last modified.
        /// </summary>
        public DateTime? LastModifiedDateUtc { get; }

    }
}
