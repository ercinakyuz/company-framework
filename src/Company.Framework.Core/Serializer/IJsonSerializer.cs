namespace Company.Framework.Core.Serializer
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        byte[] SerializeToUtf8Bytes(object value);

        TValue Deserialize<TValue>(string json);

        TValue Deserialize<TValue>(byte[] utf8Json);
    }
}
