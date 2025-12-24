namespace DummyConsoleApp.AdventOfCoding.Utilities;

public static class DataReader
{
    private const string DataFileDirectory = "AdventOfCoding\\Data";
    public static string[] ReadLines(string file, int year)
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, DataFileDirectory, $"AdventData{year}Files");
        return File.ReadAllLines(Path.Combine(filePath, file));
    }
}
