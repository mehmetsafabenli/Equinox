using System.Text.Json;
using System.Text.Json.Serialization;
using Eqn.Core.System.Collections.Generic;

namespace Eqn.Core.System.Text.Json;

public static class JsonSerializerOptionsHelper
{
    public static JsonSerializerOptions Create(JsonSerializerOptions baseOptions, JsonConverter removeConverter, params JsonConverter[] addConverters)
    {
        return Create(baseOptions, x => x == removeConverter, addConverters);
    }

    public static JsonSerializerOptions Create(JsonSerializerOptions baseOptions, Func<JsonConverter, bool> removeConverterPredicate, params JsonConverter[] addConverters)
    {
        var options = new JsonSerializerOptions(baseOptions);
        options.Converters.RemoveAll(removeConverterPredicate);
        options.Converters.AddIfNotContains(addConverters);
        return options;
    }
}
