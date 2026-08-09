namespace Company.Framework.Data.RavenDb.Serializers;

/// <summary>
/// Helper class for ULID serialization/deserialization
/// </summary>
public static class UlidSerializer
{
    public static string Serialize(object ulid)
    {
        return ulid?.ToString() ?? string.Empty;
    }

    public static object Deserialize(string value)
    {
        // ULID type is available in .NET 9+
        // For string-based representation, return as-is
        return value;
    }
}
