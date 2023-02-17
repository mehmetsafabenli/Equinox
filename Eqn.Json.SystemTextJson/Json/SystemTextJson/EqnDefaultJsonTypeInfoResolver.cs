using System.Text.Json.Serialization.Metadata;
using Eqn.Core.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Eqn.Json.SystemTextJson.Json.SystemTextJson;

public class EqnDefaultJsonTypeInfoResolver : DefaultJsonTypeInfoResolver, ITransientDependency
{
    public EqnDefaultJsonTypeInfoResolver(IOptions<EqnSystemTextJsonSerializerModifiersOptions> options)
    {
        foreach (var modifier in options.Value.Modifiers)
        {
            Modifiers.Add(modifier);
        }
    }
}
