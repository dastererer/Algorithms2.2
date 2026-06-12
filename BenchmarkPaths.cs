using System;
using System.IO;
using BenchmarkDotNet.Configs;

namespace Algorithms
{
    internal static class BenchmarkPaths
    {
        private static readonly string CommonBenchmarksRoot = Path.Combine("common", "benchmarks");
        private static readonly string ParentCommonBenchmarksRoot = Path.Combine("..", "common", "benchmarks");
        private const string ProjectFolder = "algorithms2_2";
        private const string LocalRoot = "BenchmarkDotNet.Artifacts";

        public static string ResolveBenchRootPath()
        {
            string[] roots =
            [
                Path.Combine(CommonBenchmarksRoot, ProjectFolder),
                Path.Combine(ParentCommonBenchmarksRoot, ProjectFolder),
                LocalRoot
            ];

            foreach (string root in roots)
            {
                try
                {
                    Directory.CreateDirectory(root);
                    return root;
                }
                catch
                {
                }
            }

            return LocalRoot;
        }

        public static string ResolveArtifactsPath() => Path.Combine(ResolveBenchRootPath(), "BenchmarkDotNet.Artifacts");

        public static string ResolveGraphsPath() => Path.Combine(ResolveBenchRootPath(), "graphs");
    }

    public class Algorithms22BenchmarkConfig : ManualConfig
    {
        public Algorithms22BenchmarkConfig()
        {
            ArtifactsPath = BenchmarkPaths.ResolveArtifactsPath();
        }
    }
}
