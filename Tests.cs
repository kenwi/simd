using System;
using System.Numerics;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;

namespace heightmap_simd
{
    partial class Program
    {
        private static void CreateDirectoryIfNotExists(string directory)
        {
            if(!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
        }
        private static void Test1024x768Write()
        {
            int width = 1024, height = 768;
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.Write(ref buffer, $"./data/TestWrite-{time}.png", width, height);
        }

        private static void TestFastWrite()
        {
            int width = 1024, height = 768;
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => new Rgba32((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.FastWrite(ref buffer, $"./data/FastWrite-{time}.png",  width, height);
        }
        
        private static void Test8KWrite()
        {
            int width = 7680, height = 4320;
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.Write(ref buffer, $"./data/TestWrite8K-{time}.png", width, height);
        }
        
        private static void Test8KFastWrite()
        {
           int width = 7680, height = 4320;
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => new Rgba32((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.FastWrite(ref buffer, $"./data/8KFastWrite-{time}.png",  width, height);
        }

        private static void TestCreateAndShowArray()
        {
            int width = 1024, height = 768;
            var rnd = new Random();
            Console.WriteLine($"Creating dataset {width}x{height} = {width * height} pixels");
            var a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            Console.WriteLine($"10 first values= [{string.Join(", ", a.Take(10))}]");
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