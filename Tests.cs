using System;
using System.Numerics;
using System.Linq;

namespace heightmap_simd
{
    partial class Program
    {
        private static void TestCreateAndShowArray()
        {
            int width = 1024, height = 768;
            var rnd = new Random();
            Console.WriteLine($"Creating dataset {width}x{height} = {width * height} pixels");
            var a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            Console.WriteLine($"10 first values: [{string.Join(", ", a.Take(10))}]");
        }

        private static void Test8KAddMultiply()
        {
            Console.WriteLine("SIMD Addition/Multiplication");
            var rnd = new Random();
            for (int x = 0; x < 3; x++)
            {
                var a = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();
                var b = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();

                var sw = Measure(() => SIMD.Add(ref a, ref b, out int[] result));
                Console.WriteLine($"[+][SIMD] Elapsed = {sw}");

                sw = Measure(() => SIMD.Multiply(ref a, ref b, out int[] result));
                Console.WriteLine($"[*][SIMD] Elapsed = {sw}");
            }

            Console.WriteLine("NoSIMD Addition/Multiplication");
            for (int x = 0; x < 3; x++)
            {
                var a = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();
                var b = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();

                var sw = Measure(() => NoSIMD.Add(ref a, ref b, out int[] result));
                Console.WriteLine($"[+][NoSIMD] Elapsed = {sw}");

                sw = Measure(() => NoSIMD.Multiply(ref a, ref b, out int[] result));
                Console.WriteLine($"[*][NoSIMD] Elapsed = {sw}");
            }
        }
    }
}