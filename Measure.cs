using System;
using System.Diagnostics;

namespace heightmap_simd
{
    partial class Program
    {
        private static TimeSpan Measure(Action method, bool verbose = false)
        {
            if (verbose)
                Console.WriteLine($"Running [{method.Method.Name}]");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            method();
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
    }
}