using DummyConsoleApp.AdventOfCoding.Data;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2025
{
    public class Day2CodeValidator
    {
        public void Main()
        {
            GetInvalidNumberSum("11-22", false);
            GetInvalidNumberSum("95-115", false);
            GetInvalidNumberSum(AdventData2025.Day2Codes, false);
        }

        public IEnumerable<CodeRange> ParseRanges(string data) { 
            foreach(var entry in data.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = entry.Split('-');
                yield return new CodeRange(long.Parse(parts[0]), long.Parse(parts[1]));
            }
        }

        public long GetInvalidNumberSum(string data, bool simple)
        {
            long sum = 0;
            foreach (var codeRange in ParseRanges(data))
            {
                for(long code = codeRange.Min; code <= codeRange.Max; code++)
                {
                    if (simple 
                        ? IsValidCodeSimple(code)
                        : IsValidCode(code))
                    {
                        sum += code;
                    }
                }
            }
            Console.WriteLine($"Invalid number sum: {sum}");
            return sum;
        }

        public IEnumerable<int> GetInvalidNumbers(CodeRange range)
        {

            yield return 0;

        }

        public static bool IsValidCodeSimple(long code)
        {
            var codeString = code.ToString();
            if (codeString.Length % 2 == 1)
                return false;

            var pattern = String.Concat(codeString.Take(codeString.Length / 2));
            var potentialMatch = long.Parse(pattern + pattern);
            return code == potentialMatch;
        }


        public static bool IsValidCode(long code)
        {
            var codeString = code.ToString();
            var digits = code.ToString().Take(codeString.Length/2).ToList();
            var pattern = "";
            foreach(var digit in digits)
            {
                pattern += digit;
                if(Regex.IsMatch(codeString, $@"^({pattern})+$"))
                    return true;
            }
            return false;
        }

        public class CodeRange
        {
            public long Min { get; set; }
            public long Max { get; set; }
            public CodeRange(string input)
            {

            }
            public CodeRange(long min, long max)
            {
                Min = min;
                Max = max;
            }
        }
    }
}
