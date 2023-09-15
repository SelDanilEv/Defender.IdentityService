using Defender.Common.Errors;
using Defender.Common.Exceptions;
using FluentValidation.Results;

namespace Defender.IdentityService.Application.Common.Exceptions;

public class ValidationException : ServiceException
{
    public ValidationException()
        : base(ErrorCode.VL_InvalidRequest)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(string message, IEnumerable<ValidationFailure> failures)
        : this(message)
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
