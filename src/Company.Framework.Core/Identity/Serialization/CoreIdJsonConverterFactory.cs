using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Framework.Core.Identity.Serialization;

public class CoreIdJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return IsSubclassOfRawGeneric(typeof(CoreId<,>), typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var baseType = typeToConvert.BaseType!;
        var keyType = baseType.GetGenericArguments()[0];
        var valueType = baseType.GetGenericArguments()[1];

        return (JsonConverter)Activator.CreateInstance(typeof(CoreIdJsonConverter<,>).MakeGenericType(keyType, valueType),
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
}