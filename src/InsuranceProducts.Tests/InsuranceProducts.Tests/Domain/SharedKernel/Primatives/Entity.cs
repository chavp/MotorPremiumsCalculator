using System;
using System.Collections.Generic;
using System.Text;

namespace InsuranceProducts.Tests.Domain.SharedKernel.Primatives;

/// <summary>
/// Base class for all entities in the domain.
/// Entities are objects that have identity and are distinguished by their ID rather than their attributes.
/// </summary>
/// <typeparam name="TId">The type of the entity's identifier</typeparam>
public abstract class Entity<TId> : IEntity where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Gets the date and time when this item was created.
    /// </summary>
    public DateTime CreatedDateUtc { get; private set; }

    /// <summary>
    /// Gets the date and time when this item was last modified.
    /// </summary>
    public DateTime? LastModifiedDateUtc { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Entity class with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier for this entity</param>
    protected Entity(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Parameterless constructor for Entity Framework Core.
    /// </summary>
    protected Entity()
    {
    }

    /// <summary>
    /// Determines whether two entities are equal based on their IDs.
    /// </summary>
    /// <param name="obj">The object to compare with</param>
    /// <returns>True if the entities have the same ID, false otherwise</returns>
    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    /// <summary>
    /// Returns the hash code for this entity based on its ID.
    /// </summary>
    /// <returns>A hash code for this entity</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two entities are equal.
    /// </summary>
    /// <param name="left">The first entity to compare</param>
    /// <param name="right">The second entity to compare</param>
    /// <returns>True if the entities are equal, false otherwise</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Determines whether two entities are not equal.
    /// </summary>
    /// <param name="left">The first entity to compare</param>
    /// <param name="right">The second entity to compare</param>
    /// <returns>True if the entities are not equal, false otherwise</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !Equals(left, right);
    }
}
