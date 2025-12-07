using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DummyConsoleApp.Misc
{
    internal class MIRCFileSearcher
    {
        public MIRCFileSearcher() { }
        public async Task Run(string fileName)
        {
            HashSet<string> files = new HashSet<string>();
            foreach (var line in File.ReadLines(fileName))
            {
                if (!FilterOutLine(line))
                    files.Add(line);
            }
            foreach (var line in files)
                Console.WriteLine(line);
            return;
            var categorizedFiles = BookGroupParser.GroupLines(files);
            foreach (var category in categorizedFiles.OrderBy(foo => foo.Key))
            {
                Console.WriteLine($"{category.Key}: ");
                foreach (var file in category.Value)
                    Console.WriteLine("  " + file);
            }
        }


        public bool FilterOutLine(string line, bool strict = true)
        {
            var toLowerLine = line.ToLower();
            if (!toLowerLine.Contains("epub"))
                return true;
            if (strict && !toLowerLine.Contains(".epub"))
                return true;
            return false;
        }
    }
    public class BookGroupParser
    {
        private static readonly HashSet<string> RedHerrings = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "US", "retail", "epub", "H", "UK", "SS", "SSC"
            };
        private static readonly HashSet<string> RegexRedHerrings = new HashSet<string>() {
            @"v[0-9]\.[0-9]+",
        };
        private static bool PassesExtraFilter(string text)
        {

            foreach (var regexRedHerring in RegexRedHerrings)
                if (Regex.IsMatch(text, regexRedHerring, RegexOptions.IgnoreCase))
                    return false;
            return true;
        }

        public static Dictionary<string, HashSet<string>> GroupLines(HashSet<string> lines)
        {
            // The result dictionary
            var groupedLines = new Dictionary<string, HashSet<string>>();
            var unmatchedLines = new HashSet<string>(lines);

            // Helper method to extract text within brackets
            string ExtractBracketContent(string input, char open, char close)
            {
                var start = input.IndexOf(open);
                var end = input.IndexOf(close, start + 1);
                if (start >= 0 && end > start)
                {
                    var content = input.Substring(start + 1, end - start - 1).Trim();
                    // Ensure it's not a red herring
                    if (!RedHerrings.Contains($"{content}") && PassesExtraFilter(content))
                    {
                        return content;
                    }
                }
                return null;
            }

            // Step 1: Pass for square brackets []
            foreach (var line in lines)
            {
                var bookName = ExtractBracketContent(line, '[', ']');
                if (bookName != null)
                {
                    if (!groupedLines.ContainsKey(bookName))
                    {
                        groupedLines[bookName] = new HashSet<string>();
                    }
                    groupedLines[bookName].Add(line);
                    unmatchedLines.Remove(line);
                }
            }

            // Step 2: Pass for parentheses ()
            foreach (var line in unmatchedLines.ToList())
            {
                var bookName = ExtractBracketContent(line, '(', ')');
                if (bookName != null)
                {
                    if (!groupedLines.ContainsKey(bookName))
                    {
                        groupedLines[bookName] = new HashSet<string>();
                    }
                    groupedLines[bookName].Add(line);
                    unmatchedLines.Remove(line);
                }
            }

            // Step 3: Fallback grouping for unmatched lines
            foreach (var line in unmatchedLines.ToList())
            {
                var possibleMatch = groupedLines.Keys.FirstOrDefault(bookName =>
                    line.Contains(bookName, StringComparison.OrdinalIgnoreCase));

                if (possibleMatch != null)
                {
                    groupedLines[possibleMatch].Add(line);
                    unmatchedLines.Remove(line);
                }
            }

            // Step 4: Handle unmatched lines with new logic
            foreach (var line in unmatchedLines.ToList())
            {
                // Find text starting with '-' and ending with '.', '(' or '['
                var match = Regex.Match(line, @"-([^.\[\(]+)[.\[\(]");
                if (match.Success)
                {
                    var extractedText = match.Groups[1].Value.Trim();

                    // Check if this text is contained in any categorized entry
                    var possibleMatch = groupedLines.Keys.FirstOrDefault(key =>
                        groupedLines[key].Any(lineValue => lineValue.Contains(extractedText, StringComparison.OrdinalIgnoreCase)));

                    if (possibleMatch != null)
                    {
                        groupedLines[possibleMatch].Add(line);
                        unmatchedLines.Remove(line);
                    }
                }
            }

            // Step 4: Add unmatched lines to 'unmatched' group
            if (unmatchedLines.Any())
            {
                groupedLines["_unmatched"] = new HashSet<string>(unmatchedLines);
            }

            return groupedLines;
        }
    }
}
