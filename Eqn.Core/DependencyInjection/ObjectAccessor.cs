using JetBrains.Annotations;

namespace Eqn.Core.DependencyInjection;

public class ObjectAccessor<T> : IObjectAccessor<T>
{
    public T Value { get; set; }

    public ObjectAccessor()
    {

    }

    public ObjectAccessor([CanBeNull] T obj)
    {
        Value = obj;
    }
}
