using System.ComponentModel.DataAnnotations;

namespace Eqn.Validations.Validation;

public interface IAttributeValidationResultProvider
{
    ValidationResult GetOrDefault(ValidationAttribute validationAttribute, object validatingObject, ValidationContext validationContext);
}
