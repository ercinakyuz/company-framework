using Company.Framework.Core.Id.Implementations;

namespace Company.Framework.Messaging.Envelope
{
    public record EnvelopeId(Guid Value) : IdOfGuid<EnvelopeId>(Value);
}
