using System;
using System.Numerics;
using System.Linq;

namespace heightmap_simd
{    
    partial class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            //Console.WriteLine($"Run Time = {Measure(Run8KImageTest, true)}");
            Console.WriteLine($"Run Time = {Measure(Create8KImageTest, true)}");
        }

        private static void Create8KImageTest()
        {
            var rnd = new Random();
            int width = 7680, height = 4320;
            var buffer = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 20)).ToArray();            
            
            ImageWriter.Write(ref buffer, "./Test.png", width, height);
        }
        private static void Run8KImageTest()
        {
            Console.WriteLine("SIMD Addition/Multiplication");
            var rnd = new Random();
            for (int x = 0; x < 10; x++)
            {
                var a = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();
                var b = Enumerable.Repeat(0, 7680 * 4320).Select(i => rnd.Next(0, 20)).ToArray();

                var sw = Measure(() => SIMD.Add(ref a, ref b, out int[] result));
                Console.WriteLine($"[+][SIMD] Elapsed = {sw}");
                
                sw = Measure(() => SIMD.Multiply(ref a, ref b, out int[] result));
                Console.WriteLine($"[*][SIMD] Elapsed = {sw}");
            }

            Console.WriteLine("NoSIMD Addition/Multiplication");
            for (int x = 0; x < 10; x++)
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
