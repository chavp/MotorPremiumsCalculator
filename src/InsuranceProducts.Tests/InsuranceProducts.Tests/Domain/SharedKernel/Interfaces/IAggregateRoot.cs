using InsuranceProducts.Tests.Domain.SharedKernel.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.SharedKernel.Interfaces;

/// <summary>
/// Marker interface for aggregate roots in the domain.
/// Aggregate roots are the only entities that can be directly accessed from outside their bounded context.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets the domain events that have been raised by this aggregate.
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears all domain events from this aggregate.
    /// This should be called after domain events have been processed.
    /// </summary>
    void ClearDomainEvents();
}
