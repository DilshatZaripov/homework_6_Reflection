using homework_6_Reflection.Models;
using homework_6_Reflection.Serializers;
using Newtonsoft.Json;
using System.Diagnostics;

static long Measure(Action execute)
{
    var sw = Stopwatch.StartNew();
    execute.Invoke();
    sw.Stop();
    return sw.ElapsedMilliseconds;
}

var repeatCount = 100_000;
var testF = F.Get();

var output = CSVSerializer.Serialize<F>(testF);
var outputBenchamrkTime = Measure(() =>
    {
        for (int i = 0; i < repeatCount; i++)
        {
            Console.WriteLine(output);
        }
    });

Console.Clear();
Console.WriteLine($"Console.WriteLine benchmark: {outputBenchamrkTime} ms");

Console.WriteLine("\nSerializing tests");
Measure(() => CSVSerializer.Serialize<F>(testF));
Console.WriteLine($"Custom CSV: {Measure(() => 
    { 
        for (int i = 0; i < repeatCount; i++) 
        { 
            CSVSerializer.Serialize<F>(testF); 
        } 
    })} ms");
Console.WriteLine($"Custom CSV -n/o: {Measure(() =>
{
    for (int i = 0; i < repeatCount; i++)
    {
        CSVSerializer.SerializeNotOptimized<F>(testF);
    }
})} ms");

Measure(() => JsonConvert.SerializeObject(testF));
Console.WriteLine($"Newtonsoft.Json: {Measure(() => 
    { 
        for (int i = 0; i < repeatCount; i++)
        {
            JsonConvert.SerializeObject(testF);
        }
    })} ms");

Measure(() => System.Text.Json.JsonSerializer.Serialize(testF));
Console.WriteLine($"System.Text.Json: {Measure(() =>
    {
        for (int i = 0; i < repeatCount; i++)
        {
            System.Text.Json.JsonSerializer.Serialize(testF);
        }
    })} ms");


Console.WriteLine("\nDeserializing tests");
Console.WriteLine($"Custom CSV: {Measure(() =>
    {
        for (int i = 0; i < repeatCount; i++)
        {
            CSVSerializer.Deserialize<F>("1,2,3,4,5");
        }
    })} ms");


var testJson = JsonConvert.SerializeObject(testF);
Console.WriteLine($"Newtonsoft.Json: {Measure(() =>
    {
        for (int i = 0; i < repeatCount; i++)
        {
            JsonConvert.DeserializeObject<F>(testJson);
        }
    })} ms");

Console.WriteLine($"System.Text.Json: {Measure(() =>
    {
        for (int i = 0; i < repeatCount; i++)
        {
            System.Text.Json.JsonSerializer.Deserialize<F>(testJson);
        }
    })} ms");