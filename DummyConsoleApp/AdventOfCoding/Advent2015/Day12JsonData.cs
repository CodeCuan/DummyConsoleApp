using DummyConsoleApp.AdventOfCoding.Utilities;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day12JsonData
{
    public void Main() { 
        Console.WriteLine("Day 12: JSON Data");
        var stoppy = Stopwatch.StartNew();
        var numbers =  CountNumbers("Day12JsonData.json", false);
        stoppy.Stop();
        Console.WriteLine($"Total numbers in JSON: {numbers} . Took {stoppy.ElapsedMilliseconds} ms" );
        stoppy.Restart();
        var numbersSansRed = CountNumbers("Day12JsonData.json", true);
        stoppy.Stop();
        Console.WriteLine($"Total numbers in JSON (no red): {numbersSansRed} . Took {stoppy.ElapsedMilliseconds} ms");

    }

    public int CountNumbers(string fileName, bool checkRed) { 
        var json = DataReader.ReadJToken(fileName, 2015);
        return CountNumbersInNode(json, checkRed);
    }

    public int CountNumbersInNode(JToken json, bool checkRed)
    {
        switch (json.Type)
        {
            case JTokenType.Integer:
                return json.Value<int>();
            case JTokenType.Object:
                if(checkRed && IsRedObject((JObject)json))
                    return 0;
                return json.Children().Sum(jt => CountNumbersInNode(jt, checkRed));
            case JTokenType.Array:
            case JTokenType.Property:
                return json.Children().Sum(jt => CountNumbersInNode(jt, checkRed));
            default:
                return 0; 

        }
    }
    public bool IsRedObject(JObject obj) { 
        foreach(var property in obj.Properties())
        {
            if (property.Value.Type == JTokenType.String && property.Value.Value<string>() == "red")
            {
                return true;
            }
        }
        return false;
    }
}
