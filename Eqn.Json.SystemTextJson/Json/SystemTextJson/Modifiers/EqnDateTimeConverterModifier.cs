using System.Text.Json.Serialization.Metadata;
using Eqn.Core.Reflection;
using Eqn.Json.SystemTextJson.Json.SystemTextJson.JsonConverters;
using Eqn.Timing.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace Eqn.Json.SystemTextJson.Json.SystemTextJson.Modifiers;

public class EqnDateTimeConverterModifier
{
    private IServiceProvider _serviceProvider;

    public Action<JsonTypeInfo> CreateModifyAction(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return Modify;
    }

    private void Modify(JsonTypeInfo jsonTypeInfo)
    {
        if (ReflectionHelper.GetAttributesOfMemberOrDeclaringType<DisableDateTimeNormalizationAttribute>(jsonTypeInfo.Type).Any())
        {
            return;
        }

        foreach (var property in jsonTypeInfo.Properties.Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?)))
        {
            if (property.AttributeProvider == null ||
                !property.AttributeProvider.GetCustomAttributes(typeof(DisableDateTimeNormalizationAttribute), false).Any())
            {
                property.CustomConverter = property.PropertyType == typeof(DateTime)
                    ? _serviceProvider.GetRequiredService<EqnDateTimeConverter>()
                    : _serviceProvider.GetRequiredService<EqnNullableDateTimeConverter>();
            }
        }
    }
}
