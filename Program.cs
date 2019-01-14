using System;
using System.Numerics;
using System.Diagnostics;
using System.Linq;

namespace heightmap_simd
{    
    class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            Console.WriteLine($"Run Time = {Measure(Run8KImageTest, true)}");
        }

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

        private static void RunTest2()
        {
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, 4096 * 2160).Select(i => rnd.Next(0, 20)).ToArray();
        }

        private static void Run8KImageTest()
        {
            Console.WriteLine("Running addition test");
            var rnd = new Random();
            for (int x = 0; x < 10; x++)
            {
                var a = Enumerable.Repeat(0, 4096 * 2160).Select(i => rnd.Next(0, 20)).ToArray();
                var b = Enumerable.Repeat(0, 4096 * 2160).Select(i => rnd.Next(0, 20)).ToArray();

                var sw = Measure(() => SIMD.Add(ref a, ref b, out int[] result));
                Console.WriteLine($"[0] Elapsed = {sw} SIMD");

                sw = Measure(() =>
                {
                    var result2 = new int[a.Length];
                    for (int i = 0; i < a.Length; i++)
                    {
                        result2[i] = a[i] + b[i];
                    }
                });
                Console.WriteLine($"[1] Elapsed = {sw} NO SIMD");
            }

            Console.WriteLine("Running multiplication test");
            for (int x = 0; x < 10; x++)
            {
                var a = Enumerable.Repeat(0, 4096 * 2160).Select(i => rnd.Next(0, 20)).ToArray();
                var b = Enumerable.Repeat(0, 4096 * 2160).Select(i => rnd.Next(0, 20)).ToArray();

                var sw = Measure(() => SIMD.Multiply(ref a, ref b, out int[] result));
                Console.WriteLine($"[0] Elapsed = {sw} SIMD");

                sw = Measure(() =>
                {
                    var result2 = new int[a.Length];
                    for (int i = 0; i < a.Length; i++)
                    {
                        result2[i] = a[i] * b[i];
                    }
                });
                Console.WriteLine($"[1] Elapsed = {sw} NO SIMD");
            }
        }
    }
}
