using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace DummyConsoleApp.AdventOfCoding.Utilities;

public static class DataReader
{
    private const string DataFileDirectory = "AdventOfCoding\\Data";
    public static string[] ReadLines(string file, int year)
    {
        var filePath = GetFilePath(file, year);
        return File.ReadAllLines(filePath);
    }

    public static JsonObject ReadJsonObject(string file, int year)
    {
        var filePath = GetFilePath(file, year);
        var jsonString = File.ReadAllText(filePath);
        return JsonNode.Parse(jsonString)!.AsObject();
    }

    public static JObject ReadJObject(string file, int year)
    {
        var filePath = GetFilePath(file, year);
        var jsonString = File.ReadAllText(filePath);
        return JObject.Parse(jsonString);
    }

    public static JToken ReadJToken(string file, int year)
    {
        var filePath = GetFilePath(file, year);
        var jsonString = File.ReadAllText(filePath);
        return JToken.Parse(jsonString);
    }

    private static string GetFilePath(string file, int year)
    {
        return Path.Combine(AppContext.BaseDirectory, DataFileDirectory, $"AdventData{year}Files", file);
    }
}
