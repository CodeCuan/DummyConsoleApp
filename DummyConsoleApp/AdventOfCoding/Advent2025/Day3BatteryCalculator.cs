using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2025
{
    public class Day3BatteryCalculator
    {
        public void Main()
        {
            Console.WriteLine("Day 3 Battery Calculator");
            CalculateTotalJoltageSolution1(AdventData2025.Day3BatteryBanks, 12);
        }

        public long CalculateTotalJoltageSolution1(string input, int batteryCount = 2) {
            var parsedInput = DataParser.ParseDataIntoIntLists(input, true);
            long totalVoltage = 0;
            foreach (var bank in parsedInput)
            { 
                totalVoltage += GetVoltage(bank, batteryCount);
            }
            Console.WriteLine($"Total Voltage: {totalVoltage}");
            return totalVoltage;
        }

        public long GetVoltage(IList<int> batteryBank, int batteryCount = 2) {
            var rangeOne = batteryBank.Count;
            var additionalDigits = batteryCount - 1;
            var rangeTwo = batteryBank.Take(batteryBank.Count - additionalDigits).ToList();
            var (digit, digitPos) = rangeTwo
                .Select((value, index) => (value, index))
                .MaxBy(x => x.value);
            
            var batteryVoltage = digit * (long)Math.Pow(10 ,additionalDigits);
            if (additionalDigits > 0)
                batteryVoltage+= GetVoltage(batteryBank.Skip(digitPos + 1).ToList(), additionalDigits);
            return batteryVoltage;
        }
    }
}
