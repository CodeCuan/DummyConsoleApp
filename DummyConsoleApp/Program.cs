
using DummyConsoleApp;

Console.WriteLine("Hello, World!");
Console.WriteLine("Simulating hit");
BlackJackSimulator.SimulateTests(100000, true, false);
Console.WriteLine();
Console.WriteLine("Simulating stand");
BlackJackSimulator.SimulateTests(100000, false, false);

//await new PrisonerProblemHandler(prisonerCount: 10, maxChainLength: 10000000, minChainLength: 7, logLevel:0).ProcessManyAsync(100000, 20);
