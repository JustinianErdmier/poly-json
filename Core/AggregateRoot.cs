using System.Text.Json.Serialization;

namespace Core;

[Aggregate]
[JsonPolymorphic(UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization)]
public abstract class AggregateRoot
{
    public string Id { get; set; }
}