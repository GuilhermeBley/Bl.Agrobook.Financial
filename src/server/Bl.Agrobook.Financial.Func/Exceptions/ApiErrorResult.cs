using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Bl.Agrobook.Financial.Func.Exceptions;

public record ApiErrorResult<T> : ApiErrorResult
{
    public ApiErrorResult(string ErrorMessage) : base(ErrorMessage)
    {
    }

    public ApiErrorResult(string errorMessage, string memberName) : base(errorMessage, memberName)
    {
    }

    public ApiErrorResult(string errorMessage, IEnumerable<string> memberNames) : base(errorMessage, memberNames)
    {
    }

    protected ApiErrorResult(ApiErrorResult original) : base(original)
    {
    }

    public static ApiErrorResult<T> Create<TProp>(string message, Expression<Func<T, TProp>> propertyExpression)
        => new ApiErrorResult<T>(message, GetPropertyName(propertyExpression));

    public static string GetPropertyName<TProp>(Expression<Func<T, TProp>> propertyExpression)
    {
        if (propertyExpression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        return "";
    }
}

public record ApiErrorResult(string ErrorMessage)
{
    public string[] MemberNames { get; } = [];

    public ApiErrorResult(string errorMessage, string memberName) : this(errorMessage, [memberName]) { }
    public ApiErrorResult(string errorMessage, IEnumerable<string> memberNames) : this(errorMessage)
    {
        MemberNames = memberNames.ToArray();
    }
}
