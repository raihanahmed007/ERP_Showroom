using System;

namespace ErpShowroom.Domain.Common.Exceptions;

/// <summary>
/// Exception type for domain-level validation and business rules.
/// </summary>
public class DomainException : Exception
{
    public DomainException() { }
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception innerException) : base(message, innerException) { }
}
