using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using NUlid;

namespace Company.Framework.Data.Mongo.Serializers
{
    public sealed class UlidSerializer : StructSerializerBase<Ulid>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Ulid value)
            => context.Writer.WriteString(value.ToString());

        public override Ulid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
            => Ulid.Parse(context.Reader.ReadString());
    }
}
