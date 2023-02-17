using JetBrains.Annotations;

namespace Eqn.Validations.Validation.StringValues;

public interface IStringValueType
{
    string Name { get; }

    [CanBeNull]
    object this[string key] { get; set; }

    [System.Diagnostics.CodeAnalysis.NotNull]
    Dictionary<string, object> Properties { get; }

    IValueValidator Validator { get; set; }
}
