using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Company.Framework.Core.Identity;

public class CoreIdJsonConverter<TId, TValue> : JsonConverter<TId> where TId : CoreId<TId, TValue>
{
    private readonly JsonConverter<TValue> _valueConverter;


    public CoreIdJsonConverter(JsonSerializerOptions options)
    {
        _valueConverter = (JsonConverter<TValue>)options.GetConverter(typeof(TValue));
    }

    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = _valueConverter.Read(ref reader, typeof(TValue), options)!;
        return CoreId<TId, TValue>.From(value);
    }

    public override void Write(Utf8JsonWriter writer, TId id, JsonSerializerOptions options)
    {
        _valueConverter.Write(writer, id.Value!, options);
    }
}

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

