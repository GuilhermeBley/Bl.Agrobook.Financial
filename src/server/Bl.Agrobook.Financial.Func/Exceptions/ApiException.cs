using System.Net;

namespace Bl.Agrobook.Financial.Func.Exceptions;

public class ApiException : Exception
{
    /// <summary>
    /// Error code, should be '>= 400' and '< 500'
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    /// <summary>
    /// The metadata of the error
    /// </summary>
    public List<object> ErrorResponses { get; }

    public ApiException(IEnumerable<object> errorResponses)
        : this(HttpStatusCode.BadRequest, errorResponses) { }

    public ApiException(HttpStatusCode statusCode, IEnumerable<object> errorResponses)
        : this(statusCode, errorResponses, innerException: null) { }

    public ApiException(HttpStatusCode statusCode, IEnumerable<object> errorResponses, Exception? innerException)
        : base($"Status code: {statusCode}", innerException: innerException)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(((int)statusCode), 500, paramName: "statusCode");
        ArgumentOutOfRangeException.ThrowIfLessThan(((int)statusCode), 400, paramName: "statusCode");

        StatusCode = statusCode;
        ErrorResponses = new(errorResponses);
    }
}