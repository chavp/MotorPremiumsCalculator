namespace Mti.Domain.SharedKernel.Primatives;

public interface IAuditableEntity
{
    /// <summary>
    /// Gets the date and time when this item was created.
    /// </summary>
    public DateTime CreatedDateUtc { get; }

    /// <summary>
    /// Gets the date and time when this item was last modified.
    /// </summary>
    public DateTime? LastModifiedDateUtc { get; }

    public ulong Revision { get; }
}
