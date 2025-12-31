using DummyConsoleApp.AdventOfCoding.Data;
using DummyConsoleApp.AdventOfCoding.Utilities;

namespace DummyConsoleApp.AdventOfCoding.Advent2015;

public class Day23MyComputer
{
    public void Main()
    {
        Console.WriteLine("Day 23 My Computer");
        var stoppy = System.Diagnostics.Stopwatch.StartNew();
        var registerBValue = GetRegisterBValue(AdventData2015.Day23Instructions);
        stoppy.Stop();
        Console.WriteLine($"Register B value: {registerBValue} (calculated in {stoppy.ElapsedMilliseconds} ms)");
        stoppy.Restart();
        registerBValue = GetRegisterBValue(AdventData2015.Day23Instructions, initialRegisterA: 1);
        stoppy.Stop();
        Console.WriteLine($"Register B value with initial A=1: {registerBValue} (calculated in {stoppy.ElapsedMilliseconds} ms)");
    }

    public long GetRegisterBValue(string input, int initialRegisterA = 0, bool log = false)
    {
        var instructions = DataParser.SplitLines(input).Select(line => new Instruction(line)).ToList();
        MyComputer myComputer = new();
        myComputer.Register["A"] = initialRegisterA;
        myComputer.ProcessInstructions(instructions, log);
        return myComputer.Register["B"];
    }

    private class MyComputer
    {
        public Dictionary<string, long> Register = new(StringComparer.OrdinalIgnoreCase) {
            { "A", 0 },
            { "B", 0 }
        };
        internal void ProcessInstructions(List<Instruction> instructions, bool log, int maxOperations = -1)
        {
            for (int i = 0; i < instructions.Count && maxOperations != 0;)
            {
                maxOperations--;
                var instruction = instructions[i];
                var jumps = ProcessInstruction(instruction);
                if (log)
                    Console.WriteLine($"{i} -> {i + jumps} Executed {instruction}, A={Register["A"]}, B={Register["B"]}");
                i += jumps;
            }
        }

        private int ProcessInstruction(Instruction instruction)
        {
            switch (instruction.Operation)
            {
                case Instruction.OperationType.hlf:
                    Register[instruction.Argument1] /= 2;
                    break;
                case Instruction.OperationType.tpl:
                    Register[instruction.Argument1] *= 3;
                    break;
                case Instruction.OperationType.inc:
                    Register[instruction.Argument1]++;
                    break;
                case Instruction.OperationType.jmp:
                    return instruction.Argument1Int;
                case Instruction.OperationType.jie:
                    if (Register[instruction.Argument1] % 2 == 0)
                        return instruction.Argument2;
                    break;
                case Instruction.OperationType.jio:
                    if (Register[instruction.Argument1] == 1)
                        return instruction.Argument2;
                    break;
                default:
                    throw new NotImplementedException($"Operation {instruction} not supported");
            }
            return 1;
        }
    }
    private class Instruction
    {
        public Instruction(string input)
        {
            var sections = input.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries);
            if (!Enum.TryParse(sections[0], true, out Operation))
                throw new Exception($"Invalid input {input}");
            Argument1 = sections[1];
            int.TryParse(Argument1, out Argument1Int);
            if (sections.Length > 2)
                Argument2 = int.Parse(sections[2]);
        }
        public OperationType Operation;
        public string Argument1 = "";
        public int Argument1Int = 0;
        public int Argument2 = 0;
        public enum OperationType
        {
            hlf, // half the value
            tpl, // triple the value
            inc, // increment the value by 1
            jmp, // jump to offset
            jie, // jump if even
            jio  // jump if one
        }

        public override string ToString()
        {
            return string.Join(' ', [Operation, Argument1, Argument2]);
        }
    }
}