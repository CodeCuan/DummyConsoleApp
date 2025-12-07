using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShellProgressBar;

namespace DummyConsoleApp.Misc
{

    public class PrisonerProblemHandler
    {
        int prisonerCount = 100;
        int maxChainLength = 50;
        int minChainLength = 1;
        int logLevel = 1;
        public PrisonerProblemHandler(int prisonerCount = 100, int maxChainLength = 50, int minChainLength = 1, int logLevel = 1)
        {
            this.prisonerCount = prisonerCount;
            this.maxChainLength = maxChainLength;
            this.minChainLength = minChainLength;
            this.logLevel = logLevel;
        }

        public void ProcessMany(int testCount)
        {
            List<bool> results = new List<bool>();
            using (var progressBar = new ProgressBar(testCount, "Running Tests"))
            {
                for (int i = 0; i < testCount; i++)
                {
                    results.Add(Escaped());
                    if (logLevel == 0)
                        progressBar.Tick();
                }
            }
            var statuses = Enumerable.Range(0, testCount).Select(result => Escaped()).ToList();
            var successes = statuses.Count(result => result);

            Console.WriteLine($"Tests finished! {successes} out of {statuses.Count} prisoner batches escaped.");
            Console.WriteLine($"Probability: ~{Math.Round(100M * successes / statuses.Count, 2)}%");
        }
        public async Task ProcessManyAsync(int testCount, int asyncronicity = 1)
        {
            var pollyLimiter = new SemaphoreSlim(asyncronicity);
            List<bool> results = new List<bool>();
            List<Task<bool>> taskList = new List<Task<bool>>();
            using (var progressBar = new ProgressBar(testCount, "Running Tests"))
            {

                for (int i = 0; i < testCount; i++)
                {
                    taskList.Add(Task.Run(Escaped));
                    if (taskList.Count > asyncronicity)
                    {
                        var removalTask = await Task.WhenAny(taskList);
                        results.Add(await removalTask);
                        taskList.Remove(removalTask);
                    }
                    if (logLevel == 0)
                        progressBar.Tick();
                }
            }
            foreach (var task in taskList)
                results.Add(await task);

            var successes = results.Count(result => result);

            Console.WriteLine($"Tests finished! {successes} out of {results.Count} prisoner batches escaped.");
            Console.WriteLine($"Probability: ~{Math.Round(100M * successes / results.Count, 2)}%");
        }

        public bool Escaped()
        {
            var boxes = Enumerable.Range(0, prisonerCount).Select(boxNumber => new EntityWithNumber()).ToList();
            int boxNum = 0;
            foreach (var prisoner in Enumerable.Range(0, prisonerCount).OrderBy(prisoner => Guid.NewGuid()))
            {
                boxes[boxNum].number = prisoner;
                boxNum++;
            }
            List<List<int>> chains = new List<List<int>>();
            HashSet<int> processedPrisoners = new HashSet<int>();
            foreach (var prisonerNumber in Enumerable.Range(0, prisonerCount))
            {
                if (processedPrisoners.Contains(prisonerNumber))
                    continue;
                var prisonerChain = GetChainFromNumber(prisonerNumber, prisonerNumber, boxes).ToList();
                chains.Add(prisonerChain);
                foreach (var processedPrisoner in prisonerChain)
                    processedPrisoners.Add(processedPrisoner);
            }
            var maxChain = chains.Max(chain => chain.Count);
            var minChain = chains.Min(chain => chain.Count);
            var hasEscaped = maxChainLength >= maxChain && minChainLength <= minChain;

            if (logLevel > 0)
                Console.WriteLine($"Prisoners processed through the system. Chain lengths {minChain}-{maxChain}. {(hasEscaped ? "They escaped!" : "They did not escape.")}");
            if (logLevel > 1)
            {
                Console.WriteLine("Chains:");
                foreach (var chain in chains.OrderByDescending(chain => chain.Count))
                    Console.WriteLine(string.Join(",", chain));
            }
            if (logLevel > 2)
            {
                Console.WriteLine("Boxes: ");
                foreach (var boxIndex in Enumerable.Range(0, prisonerCount))
                {
                    Console.WriteLine($"{boxIndex}:{boxes[boxIndex].number}");
                }
            }
            return hasEscaped;
        }
        private IEnumerable<int> GetChainFromNumber(int seedNum, int activeNum, List<EntityWithNumber> boxes)
        {
            var box = boxes[activeNum];
            yield return box.number;
            if (box.number != seedNum)
                foreach (var chainNum in GetChainFromNumber(seedNum, box.number, boxes))
                    yield return chainNum;
        }
        class EntityWithNumber
        {
            public EntityWithNumber() { }
            public EntityWithNumber(int boxNum)
            {
                number = boxNum;
            }
            public int number = 0;
        }
    }

}
