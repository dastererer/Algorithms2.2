using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
    class Naive
    {
        public static string[] RunAlgorithm(string text, string[] wordList)
        {
            var wrongSpells = new List<string>();
            string[] words = text.Split(Program.delimiters, StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (!wordList.Contains(word, StringComparer.OrdinalIgnoreCase))
                {
                    wrongSpells.Add(word);
                }
            }
            return wrongSpells.ToArray(); 
        }
    }
}