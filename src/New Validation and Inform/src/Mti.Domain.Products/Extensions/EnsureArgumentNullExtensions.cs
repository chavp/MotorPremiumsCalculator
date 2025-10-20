using EnsureThat;
using Mti.Domain.Products.ValueObjects;

namespace Mti.Domain.Products.Extensions;

public static class EnsureArgumentNullExtensions
{
    public static Code NotEmpty(this Param<Code> param,
        string message, string paramName)
    {
        param.IsNotNull();
        return !param.Value.IsEmpty
            ? param.Value
            : throw Ensure.ExceptionFactory.ArgumentNullException(message, paramName);
    }

    public static Name NotEmpty(this Param<Name> param,
        string message, string paramName)
    {
        param.IsNotNull();
        return !param.Value.IsEmpty
            ? param.Value
            : throw Ensure.ExceptionFactory.ArgumentNullException(message, paramName);
    }

    public static Guid NotEmpty(this Param<Guid> param,
        string message, string paramName)
        => param.Value != Guid.Empty
               ? param.Value
               : throw Ensure.ExceptionFactory.ArgumentNullException(message, paramName);
}
