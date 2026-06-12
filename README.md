# Algorithms 2.2

A C#/.NET 8 project focused on practical implementations of classic data structures, graph algorithms, and benchmark-driven comparison.

The project is organized around three engineering themes:

- dictionary-backed spell checking
- graph traversal for pathfinding and partitioning
- custom hash table and prefix-tree implementations with runtime and memory comparison

This repository is not just a collection of assignment outputs. Its main value is in the implementations themselves: how different structures are built, how they are evaluated, and how the same problem changes when the underlying data structure changes.

## Tech Stack

- C#
- .NET 8
- BenchmarkDotNet
- Python 3
- matplotlib

## Project Structure

- [Program.cs](c:/Develop/University/Algorithms2.2/Program.cs): console entry point with task-oriented menu and benchmark launch options
- [Tools.cs](c:/Develop/University/Algorithms2.2/Tools.cs): data loading and shared preprocessing utilities
- [Algorithms](c:/Develop/University/Algorithms2.2/Algorithms): task-level algorithm wrappers
- [Structures](c:/Develop/University/Algorithms2.2/Structures): custom data structures
- [data](c:/Develop/University/Algorithms2.2/data): dictionaries and benchmark texts
- [BenchmarkDotNet.Artifacts](c:/Develop/University/Algorithms2.2/BenchmarkDotNet.Artifacts): benchmark outputs
- [plot_benchmarks.py](c:/Develop/University/Algorithms2.2/plot_benchmarks.py): CSV-to-graph pipeline

## Implemented Components

### Spell Checking Strategies

The spell-checking pipeline compares multiple dictionary representations over the same input text.

Implemented strategies:

- naive linear lookup
- hash-based lookup via `HashSet`
- RB-tree lookup via a custom red-black tree
- ordinary trie
- balanced ternary search tree
- double-array trie

Relevant files:

- [Algorithms/Naive.cs](c:/Develop/University/Algorithms2.2/Algorithms/Naive.cs)
- [Algorithms/HashMap.cs](c:/Develop/University/Algorithms2.2/Algorithms/HashMap.cs)
- [Algorithms/BBST.cs](c:/Develop/University/Algorithms2.2/Algorithms/BBST.cs)
- [Algorithms/Trie.cs](c:/Develop/University/Algorithms2.2/Algorithms/Trie.cs)
- [Algorithms/TernaryTrieAlg.cs](c:/Develop/University/Algorithms2.2/Algorithms/TernaryTrieAlg.cs)
- [Algorithms/DoubleArrayTrieAlg.cs](c:/Develop/University/Algorithms2.2/Algorithms/DoubleArrayTrieAlg.cs)

Supporting structures:

- [Structures/RedBlackTree.cs](c:/Develop/University/Algorithms2.2/Structures/RedBlackTree.cs)
- [Structures/Trie.cs](c:/Develop/University/Algorithms2.2/Structures/Trie.cs)
- [Structures/TST.cs](c:/Develop/University/Algorithms2.2/Structures/TST.cs)
- [Structures/DoubleArrayTrie.cs](c:/Develop/University/Algorithms2.2/Structures/DoubleArrayTrie.cs)

### Graph Algorithms

#### Triwizard Tournament

The labyrinth task is solved with a single BFS over the map. That is the correct model because BFS on an unweighted grid gives shortest path lengths to every reachable cell in one traversal. Once distances are known, each wizard's finish time is computed as:

$time = \frac{shortest\ path\ length}{speed}$

Relevant file:

- [Algorithms/BFS2D.cs](c:/Develop/University/Algorithms2.2/Algorithms/BFS2D.cs)

#### Aunt's Namesday

This task is not just "some DFS". It is a graph bipartition problem.

Each guest is a vertex.
Each mutual dislike relation is an edge.
The question is whether the graph is bipartite.

The implementation uses non-recursive DFS with an explicit stack and assigns one of two table labels to each guest. If an edge connects two guests already assigned to the same table, the arrangement is impossible.

That is why the DFS is the right explanation here: the algorithm is not searching for a path, it is checking whether a two-coloring exists.

Relevant file:

- [Algorithms/PartyPlanner.cs](c:/Develop/University/Algorithms2.2/Algorithms/PartyPlanner.cs)

### Hash Tables

Three hash table variants are implemented and benchmarked against load factor:

- separate chaining
- linear probing
- double hashing

Relevant files:

- [Structures/ChainingHashTable.cs](c:/Develop/University/Algorithms2.2/Structures/ChainingHashTable.cs)
- [Structures/LinearProbingHashTable.cs](c:/Develop/University/Algorithms2.2/Structures/LinearProbingHashTable.cs)
- [Structures/DoubleHashingHashTable.cs](c:/Develop/University/Algorithms2.2/Structures/DoubleHashingHashTable.cs)

The benchmark setup was adjusted so that:

- successful search only uses keys actually inserted into the table
- failed search uses guaranteed-missing keys
- insert benchmarks measure insertion of new keys rather than updates of existing entries

## Benchmarks

### Spell Checker Benchmarks

File:

- [BenchmarksSpellCheckers.cs](c:/Develop/University/Algorithms2.2/BenchmarksSpellCheckers.cs)

Measures:

- dictionary build time
- lookup time

Across:

- naive
- hash map
- RB-tree
- ordinary trie
- ternary trie
- DAT

### Hash Table Benchmarks

File:

- [BenchmarkHashTables.cs](c:/Develop/University/Algorithms2.2/BenchmarkHashTables.cs)

Measures:

- successful search
- failed search
- insert

Against:

- load factor from `0.05` to `0.95`

### Dictionary Races Benchmarks

File:

- [BenchmarkDictionaryRaces.cs](c:/Develop/University/Algorithms2.2/BenchmarkDictionaryRaces.cs)

Measures:

- build time
- lookup time
- memory usage

Across:

- RB-tree
- ordinary trie
- balanced ternary trie
- double-array trie

For dictionary sizes:

- `5000`
- `10000`
- `20000`

## Console Interface

The project now has a console menu oriented around the implemented tasks instead of raw benchmark classes.

Run:

```powershell
dotnet run
```

All commands below assume you run them from the repository root.

Available menu groups:

- Part One
- Part Two
- Part Three
- Run All Benchmarks

Direct command arguments are also supported:

```powershell
dotnet run -- part1a
dotnet run -- part1b
dotnet run -- part1c
dotnet run --configuration Release -- part1-benchmarks
dotnet run --configuration Release -- part4
dotnet run --configuration Release -- part5
dotnet run --configuration Release -- all-benchmarks
```

## Generating Graphs

The Python script reads BenchmarkDotNet CSV files and builds charts with matplotlib.

Run:

```powershell
py -3 plot_benchmarks.py
```

Generated output is written to:

- `graphs/`

Current graph groups supported by the script:

- spell checker build time
- spell checker lookup time
- hash table insert time
- hash table successful search time
- hash table failed search time
- dictionary race build time
- dictionary race lookup time
- dictionary race build memory

## Data Files

Main inputs used by the project:

- [data/dictionaries/english_words.txt](c:/Develop/University/Algorithms2.2/data/dictionaries/english_words.txt)
- [data/dictionaries/english_names.txt](c:/Develop/University/Algorithms2.2/data/dictionaries/english_names.txt)
- [data/texts/little_prince__with_errors.txt](c:/Develop/University/Algorithms2.2/data/texts/little_prince__with_errors.txt)
- [data/texts/war_and_peace.txt](c:/Develop/University/Algorithms2.2/data/texts/war_and_peace.txt)
- [data/texts/party.txt](c:/Develop/University/Algorithms2.2/data/texts/party.txt)

## Build and Run

Build:

```powershell
dotnet build
```

Run interactive menu:

```powershell
dotnet run
```

Run hash table benchmarks only:

```powershell
dotnet run --configuration Release -- part4
```

Run dictionary race benchmarks only:

```powershell
dotnet run --configuration Release -- part5
```

Run all benchmarks:

```powershell
dotnet run --configuration Release -- all-benchmarks
```

Generate plots from CSV:

```powershell
py -3 plot_benchmarks.py
```

## Notes

- Benchmark CSV and HTML files are produced by BenchmarkDotNet under `BenchmarkDotNet.Artifacts`.
- The `graphs/` folder contains generated images, not source code.
- Some nullable warnings still exist in older hash table files, but the project builds and runs correctly.
