using Company.Framework.Core.Id.Abstractions;
using Company.Framework.Core.Id.Implementations;
using Company.Framework.Core.Monad;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Framework.Core.Id.Serialization;

public class IdJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return HasImplementedGenericInterface(typeToConvert, typeof(IId<,>));
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var implemented = GetImplementedGenericInterface(typeToConvert, typeof(IId<,>));
        var keyType = implemented.GetGenericArguments()[0];
        var valueType = implemented.GetGenericArguments()[1];

        return (JsonConverter)Activator.CreateInstance(typeof(IdJsonConverter<,>).MakeGenericType(keyType, valueType),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object[] { options },
            culture: null)!;
    }

    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }

    static Type GetImplementedGenericInterface(Type derivedType, Type implementedType)
    {
        return derivedType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == implementedType);
    }

    static bool HasImplementedGenericInterface(Type derivedType, Type implementedType)
    {
        return derivedType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == implementedType);
    }
}