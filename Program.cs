using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using BenchmarkDotNet.Running;

namespace Algorithms
{
    class Program
    {
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
            BenchmarkRunner.Run<BenchmarkHashTables>();
        }
    }
}
