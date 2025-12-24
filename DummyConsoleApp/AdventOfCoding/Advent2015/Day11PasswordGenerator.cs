
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DummyConsoleApp.AdventOfCoding.Advent2015
{
    public class Day11PasswordGenerator
    {
        public void Main()
        {
            string initialPassword = "hxbxwxba";
            Stopwatch stoppy = Stopwatch.StartNew();
            for (int i = 0; i < 20; i++)
            {
                initialPassword= GetNextPassword(initialPassword);
                Console.WriteLine($"Next valid password: {initialPassword} took {stoppy.ElapsedMilliseconds} ms");
                stoppy.Restart();
            }
        }

        public string GetNextPassword(string input)
        {
            var passwordChars = input.ToCharArray();
            while (true) {
                input = IncrementPassword(input);
                if(PasswordIsValid(input))
                    return input;
            }
        }

        private static Regex DoubleLetterRegex = new(@"([a-z])\1.*([a-z])\2", RegexOptions.Compiled);
        private static Regex vowelRegex = new(@"[iol]", RegexOptions.Compiled);

        private bool PasswordIsValid(string input)
        {
            if(!DoubleLetterRegex.IsMatch(input)
                || vowelRegex.IsMatch(input))
                return false;
            List<int> letterChain = [];
            foreach(var c in input)
            {
                if(letterChain.Count == 0
                    || letterChain.Last() + 1 == c)
                {
                    letterChain.Add(c);
                    if (letterChain.Count >= 3)
                        return true;
                }
                else
                {
                    letterChain = [c];
                }
            }
            return false;
        }

        private string IncrementPassword(string input)
        {
            for(int i = input.Length - 1; i >= 0; i--)
            {
                if (input[i] == 'z')
                {
                    input = input.Substring(0, i) + 'a' + input.Substring(i + 1);
                }
                else
                {
                    input = input.Substring(0, i)
                        + (char)(input[i] + 1)
                        + input.Substring(i + 1);
                    break;
                }
            }
            return input;
        }
    }
}
