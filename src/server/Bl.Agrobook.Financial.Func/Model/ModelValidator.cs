using System.ComponentModel.DataAnnotations;

namespace Bl.Agrobook.Financial.Func.Model;

internal class ModelValidator
{
    public static IReadOnlyList<ValidationResult> Validate<T>(T model) where T : notnull
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);

        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        if (!isValid)
        {
            return validationResults;
        }

        return [];
    }
}
