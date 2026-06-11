using System.Collections.Generic;

namespace Algorithms.Graphs
{
    public class PartyPlanner
    {
        internal static bool RunAlgorithm(Tools tools, out Dictionary<string, int> seating)
        {
            seating = new Dictionary<string, int>();

            foreach (var guest in tools.PartyGuests)
            {
                seating[guest] = 0; 
            }

            foreach (var startGuest in tools.PartyGuests)
            {
                if (seating[startGuest] == 0)
                {
                    var stack = new Stack<string>();
                    stack.Push(startGuest);
                    seating[startGuest] = 1; // Стол №1

                    while (stack.Count > 0)
                    {
                        string current = stack.Pop();
                        int currentTable = seating[current];

                        foreach (var enemy in tools.PartyGraph[current])
                        {
                            if (seating[enemy] == 0)
                            {
                                seating[enemy] = -currentTable; 
                                stack.Push(enemy);
                            }
                            else if (seating[enemy] == currentTable)
                            {
                                return false; 
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}