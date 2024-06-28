// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Core;

var options = new JsonSerializerOptions
{
    TypeInfoResolver = new PolymorphicTypeResolver<AggregateRoot, AggregateAttribute>(
        typeof(Lib1.Class1).Assembly,
        typeof(Lib2.Class1).Assembly
    )
};


var root = new Root()
{
    Lib1 = new Lib1.Class1() { Id = "Lib1", Number = 123 },
    Lib2 = new Lib2.Class1() { Id = "Lib2", Name = "name" }
};

string result = JsonSerializer.Serialize(root, options);
Console.WriteLine(result);
var root1 = JsonSerializer.Deserialize<Root>(result);
Console.WriteLine(root1!.Lib1.Number);
Console.WriteLine(root1.Lib2.Name);
