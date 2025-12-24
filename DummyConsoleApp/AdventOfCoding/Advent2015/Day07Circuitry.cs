using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;
using DummyConsoleApp.AdventOfCoding.Utilities.Extensions;
using System.Collections;
using System.Diagnostics;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day07Circuitry
{
    public void Main()
    {
        Console.WriteLine("Day 7 Circuitry");
        var stoppy = Stopwatch.StartNew();
        var signal = GetSignal(AdventData2015.Day7Circuitry);
        stoppy.Stop();
        Console.WriteLine($"Signal on wire a: {signal} . Took {stoppy.ElapsedMilliseconds} ms");

        stoppy.Restart();
        signal = GetSignalDoubleParse(AdventData2015.Day7Circuitry);
        stoppy.Stop();
        Console.WriteLine($"New signal on wire a: {signal} . Took {stoppy.ElapsedMilliseconds} ms");

    }

    public void LogSignal(string input)
    {
        var wires = DataParser.SplitLines(input)
            .Select(line => new Wire(line))
            .ToDictionary(wire => wire.key, wire => wire);
        foreach (var wireKey in wires.Keys.Order())
        {
            var bitVal = wires[wireKey].GetSignalValue(wires);
            Console.WriteLine($"{wireKey}: {bitVal.ToInt()}");
        }
    }

    public int GetSignalDoubleParse(string input)
    {
        var wires = DataParser.SplitLines(input)
            .Select(line => new Wire(line))
            .ToDictionary(wire => wire.key, wire => wire);
        var bitValue = wires["a"].GetSignalValue(wires);
        foreach (var wireSet in wires) { 
            if(wireSet.Key == "b")
                wireSet.Value.SignalBits = new BitArray(bitValue);
            else
                wireSet.Value.SignalBits = null;
        }
        bitValue = wires["a"].GetSignalValue(wires);
        return bitValue.ToInt();
    }

    public int GetSignal(string input)
    {
        var wires = DataParser.SplitLines(input)
            .Select(line => new Wire(line))
            .ToDictionary(wire => wire.key, wire => wire);
        var bitValue = wires["a"].GetSignalValue(wires);
        return bitValue.ToInt();
    }

    public class Wire
    {
        public string key;
        public BitArray? SignalBits;
        public List<string> arguments = [];
        public List<BitArray> processedArguments = [];
        public WireOperator wireOperator = WireOperator.None;
        public Wire(string line)
        {
            var parts = line.Split("->",
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            key = parts[1];
            var instructionSet = parts[0].Split(' ',
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            switch (instructionSet.Length)
            {
                case 1:
                    arguments.Add(instructionSet.First());
                    break;
                case 2:
                    ParseTwoArgumentSet(instructionSet);
                    break;
                case 3:
                    ParseThreeArgumentSet(instructionSet);
                    break;
                default:
                    throw new Exception($"Invalid instruction set: {line}");
            }
        }

        private void ParseTwoArgumentSet(string[] wireSections)
        {
            wireOperator = Enum.Parse<WireOperator>(wireSections[0], true);
            arguments.Add(wireSections[1]);
        }

        private void ParseThreeArgumentSet(string[] wireSections)
        {
            wireOperator = Enum.Parse<WireOperator>(wireSections[1], true);
            arguments.Add(wireSections[0]);
            arguments.Add(wireSections[2]);
        }

        public BitArray GetSignalValue(Dictionary<string, Wire> wires)
        {
            SignalBits ??= CalculateSignalValue(wires);
            return SignalBits;

        }

        public BitArray CalculateSignalValue(Dictionary<string, Wire> wires)
        {
            processedArguments = arguments
                .Select(arg => GetValue(arg, wires))
                .ToList();
            switch (wireOperator)
            {
                case WireOperator.Not:
                    return processedArguments[0].Not();
                case WireOperator.And:
                    return processedArguments[0].And(processedArguments[1]);
                case WireOperator.Or:
                    return processedArguments[0].Or(processedArguments[1]);
                case WireOperator.LShift:
                    return processedArguments[0].LeftShift(processedArguments[1].ToInt());
                case WireOperator.RShift:
                    return processedArguments[0].RightShift(processedArguments[1].ToInt());
                case WireOperator.None:
                    return processedArguments[0];
                default:
                    throw new NotImplementedException($"Operator {wireOperator} not implemented");
            }
        }

        private BitArray GetValue(string input, Dictionary<string, Wire> wires)
        {
            if (int.TryParse(input, out int value))
                return CreateBitArray(value, 16);
            return new BitArray(wires[input].GetSignalValue(wires));
        }

        private BitArray CreateBitArray(int value, int length)
        {
            BitArray source = new BitArray(new[] { value });
            BitArray result = new BitArray(length);

            int copyLength = Math.Min(length, source.Length);
            for (int i = 0; i < copyLength; i++)
                result[i] = source[i];
            return result;
        }

        public enum WireOperator
        {
            None,
            And,
            Or,
            RShift,
            LShift,
            Not
        }
    }
}
