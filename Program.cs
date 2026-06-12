using Algorithms.Graphs;
using BenchmarkDotNet.Running;
using Algorithms;

class Program
{
    static void TestSpellChecker()
    {
        Tools tools = new Tools();
        tools.pathToText = @"data\texts\little_prince__with_errors.txt";
        tools.pathToWordList = @"data\dictionaries\english_words.txt";
        tools.pathToNamesList = @"data\dictionaries\english_names.txt";

        var naiveResult = Naive.RunAlgorithm(tools.text, tools.wordList, tools.namesList);
        var bbstResult = BBST.RunAlgorithm(tools.text, tools.wordList, tools.namesList);
        var hashMapResult = HashMap.RunAlgorithm(tools.text, tools.wordList, tools.namesList);
        var trieResult = TrieAlg.RunAlgorithm(tools.text, tools.wordList, tools.namesList);

        Console.WriteLine($"Spell Checker Results:");
        Console.WriteLine($"Naive: {naiveResult.Length} errors");
        Console.WriteLine($"BBST: {bbstResult.Length} errors");
        Console.WriteLine($"HashMap: {hashMapResult.Length} errors");
        Console.WriteLine($"Trie: {trieResult.Length} errors");
        Console.WriteLine();
    }

    static void TestLabyrinth()
    {
        char[,] labyrinth = new char[,]
        {
            { '.', '.', '#', '.', '.' },
            { '#', '.', '#', '.', '#' },
            { '.', '.', '.', '.', '.' },
            { '#', '#', '.', '#', '.' },
            { '.', '.', '.', '.', 'E' }
        };

        (int winner, double time) result = FindLabyrinthWinner(
            labyrinth,
            (4, 4),
            (0, 0),
            (0, 4),
            (4, 0),
            3, 2, 2
        );

        Console.WriteLine($"Labyrinth Results:");
        Console.WriteLine($"Winner: Wizard {result.winner}");
        Console.WriteLine($"Time: {result.time:F2} minutes");
        Console.WriteLine();
    }

    static void TestParty()
    {
        Tools tools = new Tools();
        tools.pathToPartyData = @"data\texts\party.txt";

        if (PartyPlanner.RunAlgorithm(tools, out var seatingPlan))
        {
            Console.WriteLine("Party Seating Results:");
            foreach (var kvp in seatingPlan)
            {
                Console.WriteLine($"{kvp.Key}: Table {(kvp.Value > 0 ? 1 : 2)}");
            }
        }
        else
        {
            Console.WriteLine("Impossible to arrange seating!");
        }
        Console.WriteLine();
    }

    static (int winner, double time) FindLabyrinthWinner(char[,] labyrinth, (int x, int y) finishPos, (int x, int y) wizardOne, (int x, int y) wizardTwo, (int x, int y) wizardThree, int wizardOneSpeed, int wizardTwoSpeed, int wizardThreeSpeed)
    {
        int[,] distMap = BFS2D.RunAlgorithm(labyrinth, finishPos);
        double time1 = (double)distMap[wizardOne.x, wizardOne.y] / wizardOneSpeed;
        double time2 = (double)distMap[wizardTwo.x, wizardTwo.y] / wizardTwoSpeed;
        double time3 = (double)distMap[wizardThree.x, wizardThree.y] / wizardThreeSpeed;
        double[] results = { time1, time2, time3 };
        int winnerIndex = Array.IndexOf(results, results.Min());
        
        return (winner: winnerIndex + 1, time: results.Min());
    }

    static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            RunFromArgs(args[0]);
            return;
        }

        ShowMainMenu();
    }

    static void RunFromArgs(string command)
    {
        switch (command.ToLowerInvariant())
        {
            case "part1a":
                TestSpellChecker();
                break;
            case "part1b":
                TestLabyrinth();
                break;
            case "part1c":
                TestParty();
                break;
            case "part1-benchmarks":
                RunSpellCheckerBenchmarks();
                break;
            case "part2":
            case "part4":
                RunHashTableBenchmarks();
                break;
            case "part3":
            case "part5":
                RunDictionaryRacesBenchmarks();
                break;
            case "all-benchmarks":
                RunAllBenchmarks();
                break;
            default:
                ShowMainMenu();
                break;
        }
    }

    static void ShowMainMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Part One");
            Console.WriteLine("2. Part Two");
            Console.WriteLine("3. Part Three");
            Console.WriteLine("4. Run All Benchmarks");
            Console.WriteLine("0. Exit");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    ShowPartOneMenu();
                    break;
                case "2":
                    ShowPartTwoMenu();
                    break;
                case "3":
                    ShowPartThreeMenu();
                    break;
                case "4":
                    RunAllBenchmarks();
                    return;
                case "0":
                    return;
            }
        }
    }

    static void ShowPartOneMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Me spell rite");
            Console.WriteLine("2. Me spell rite benchmarks");
            Console.WriteLine("3. Triwizard Tournament");
            Console.WriteLine("4. Aunt's Namesday");
            Console.WriteLine("5. Run all Part One tasks");
            Console.WriteLine("0. Back");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    TestSpellChecker();
                    break;
                case "2":
                    RunSpellCheckerBenchmarks();
                    return;
                case "3":
                    TestLabyrinth();
                    break;
                case "4":
                    TestParty();
                    break;
                case "5":
                    TestSpellChecker();
                    TestLabyrinth();
                    TestParty();
                    break;
                case "0":
                    return;
            }
        }
    }

    static void ShowPartTwoMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Full house benchmarks");
            Console.WriteLine("0. Back");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunHashTableBenchmarks();
                    return;
                case "0":
                    return;
            }
        }
    }

    static void ShowPartThreeMenu()
    {
        while (true)
        {
            Console.WriteLine("1. Dictionary races benchmarks");
            Console.WriteLine("0. Back");
            Console.Write("Choose option: ");

            string? choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunDictionaryRacesBenchmarks();
                    return;
                case "0":
                    return;
            }
        }
    }

    static void RunSpellCheckerBenchmarks()
    {
        BenchmarkRunner.Run<BenchmarkSpellCheckers>();
    }

    static void RunHashTableBenchmarks()
    {
        BenchmarkRunner.Run<BenchmarkHashTables>();
    }

    static void RunDictionaryRacesBenchmarks()
    {
        BenchmarkRunner.Run<BenchmarkDictionaryRaces>();
    }

    static void RunAllBenchmarks()
    {
        RunSpellCheckerBenchmarks();
        RunHashTableBenchmarks();
        RunDictionaryRacesBenchmarks();
    }
}

