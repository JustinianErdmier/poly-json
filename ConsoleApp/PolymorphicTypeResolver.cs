using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Core;

public class PolymorphicTypeResolver<T, TK> : DefaultJsonTypeInfoResolver
    where TK : Attribute
{
    private Assembly[] _assembliesToScan;

    public PolymorphicTypeResolver(params Assembly[] assembliesToScan)
    {
        _assembliesToScan = assembliesToScan;
    }

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);

        var types = _assembliesToScan
            .SelectMany(a =>
                a
                    .GetTypes()
                    .Where(t => t.GetCustomAttributes(typeof(TK), inherit: true).Any())
                    .ToList());

        List<JsonDerivedType> derivedTypes = new();
        foreach (var type1 in types)
        {
            derivedTypes.Add(new JsonDerivedType(type1));
        }


        Type basePointType = typeof(T);
        if (jsonTypeInfo.Type == basePointType)
        {
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "__type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            };
            foreach (var derivedType in derivedTypes)
            {
                jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(derivedType);
            }
        }

        return jsonTypeInfo;
    }
}