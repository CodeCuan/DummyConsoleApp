using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;


namespace DummyConsoleApp.AdventOfCoding.Advent2024;

public class Advent2024Solution2
{
    public List<List<int>> data;
    public Advent2024Solution2()
    {
        data = DataParser.ParseDataIntoIntLists(AdventData2024.PuzzleTwoReportData);
    }

    public void Main()
    {
        CountSafeReports();
    }

    public void CountSafeReports()
    {
        var safeReports = data.Count(IsSafeReport);
        Console.WriteLine(safeReports);
    }

    public bool IsSafeReportBasic(List<int> report)
    {
        if (report.Count < 2)
            return true;
        if (report[1] == report[0]) return false;

        var isValid = IsAscendingReportBasic(report, report[1] > report[0]);

        return isValid;
    }

    public bool IsSafeReport(List<int> report)
    {
        var isValid = IsAscendingReport(report, true)
            || IsAscendingReport(report, false);

        return isValid;
    }

    public bool IsAscendingReport(List<int> report, bool ascending)
    {
        if (report.Count < 3)
            return true;

        bool usedSafety = false;
        for (int i = 0; i < report.Count - 2; i++)
        {
            if (ValidNumbers(report[i + 1], report[i], ascending))
                continue;
            if (usedSafety)
                return false;
            usedSafety = true;
            if (ValidNumbers(report[i + 2], report[i], ascending))
            {
                i++; // skip next number
                if (i + 2 == report.Count)
                    return true;
            }
            else if (i > 0 && !ValidNumbers(report[i + 1], report[i - 1], ascending))
            {
                return false;
            }
        }

        return !usedSafety || ValidNumbers(report[report.Count - 1], report[report.Count - 2], ascending);
    }

    public bool IsAscendingReportBasic(List<int> report, bool ascending)
    {
        int? prevNumber = null;
        foreach (var reportNum in report)
        {
            if (prevNumber.HasValue
                && !ValidNumbers(reportNum, prevNumber.Value, ascending))
                return false;
            prevNumber = reportNum;
        }
        return true;
    }

    public bool ValidNumbers(int current, int previous, bool ascending)
    {
        if (ascending)
            return previous + 3 >= current && current > previous;
        else
            return previous - 3 <= current && current < previous;
    }


}
