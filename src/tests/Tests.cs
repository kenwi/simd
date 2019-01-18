using System;
using System.Numerics;
using System.Linq;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace simd
{
    partial class Program
    {
        static int width = 1920;
        static int height = 1080;

        private static void CreateDirectoryIfNotExists(string directory)
        {
            if (!System.IO.Directory.Exists(directory))
                System.IO.Directory.CreateDirectory(directory);
        }

        private static IEnumerable<Rgba32> GetRandomSet(int num)
        {
            var rnd = new Random();
            return Enumerable.Repeat(0, num).Select(i => new Rgba32((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255)));;
        }

        private static void TestGetRandomSurfaceField()
        {
            var grid = new int[1000];
            var rnd = new Random();
            var geometry = new SphericalGeometry();
            var zMultipliers = geometry.GetRandomSurfaceField(grid, rnd);
            Console.WriteLine($"[10] first values of [zMultipliers] = [{string.Join(", ", zMultipliers.Take(10))}]");
        }

        private static void TestExecuteOnSetsMethod()
        {
            int printNumberOfValues = 5, testNumberOfRuns = 5;
            var rnd = new Random();
            int[] result = null;
            var a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(1, 255)).ToArray();
            var b = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(1, 255)).ToArray();
            
            Console.WriteLine($"[{printNumberOfValues}] first values of [a] = [{string.Join(", ", a.Take(printNumberOfValues))}]");
            Console.WriteLine($"[{printNumberOfValues}] first values of [b] = [{string.Join(", ", b.Take(printNumberOfValues))}]");
            for (int x = 0; x < testNumberOfRuns; x++)
            {
                var sw = Measure(() => SIMD.ExecuteOnSets(ref a, ref b, ref result, (va, vb) => va + vb));
                Console.WriteLine($"[+] [{sw}] [{printNumberOfValues}] first values of [result] = [{string.Join(", ", result.Take(printNumberOfValues))}]");

                sw = Measure(() => SIMD.ExecuteOnSets(ref a, ref b, ref result, (va, vb) => va - vb));
                Console.WriteLine($"[-] [{sw}] [{printNumberOfValues}] first values of [result] = [{string.Join(", ", result.Take(printNumberOfValues))}]");
                
                sw = Measure(() => SIMD.ExecuteOnSets(ref a, ref b, ref result, (va, vb) => va * vb));
                Console.WriteLine($"[*] [{sw}] [{printNumberOfValues}] first values of [result] = [{string.Join(", ", result.Take(printNumberOfValues))}]");
                
                sw = Measure(() => SIMD.ExecuteOnSets(ref a, ref b, ref result, (va, vb) => va / vb));
                Console.WriteLine($"[/] [{sw}] [{printNumberOfValues}] first values of [result] = [{string.Join(", ", result.Take(printNumberOfValues))}]");
            }
        }

        private static void TestGenerateBasicMap()
        {
            var buffer = new Rgba32[width * height];
            var rnd = new Random();
            var continent = new FastNoise();
            continent.SetFractalOctaves(6);
            continent.SetFrequency(0.0007f);

            for(int i=0; i<buffer.Length; i++)
            {
                var p0 = ArrayIndex.From1DTo2D(i, width);
                var pixelValue = continent.GetCellular(p0.X, p0.Y);

                if(pixelValue > 0)
                {
                    buffer[i] = new Rgba32(pixelValue, 10 + 1.0f * pixelValue, 0.0f);
                }
                else
                    buffer[i] = new Rgba32(pixelValue, 0.0f, 10 + 1.0f *  pixelValue);
            }

            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");
            CreateDirectoryIfNotExists("./data");
            ImageWriter.FastWrite(ref buffer, $"./data/TestGenerateBasicMap-{time}.png", width, height);
        }

        private static void TestIntensityImage()
        {
            var buffer = new Rgba32[width * height];
            var rnd = new Random();
            var fn = new FastNoise();

            Func<double, double, double, double> getCentralIntensity = (r, i0, k) => i0 * Math.Exp(-k * Math.Pow(r, 0.25));
            Func<double, double, double, double> getOuterIntensity = (r, i0, a) => i0 * Math.Exp(-r / a);
            Func<double, double> getSurfaceBrightness = (r) => {
                double i0 = 1.0, k = 0.02, a = 200;
                double bulgeradius = (width + height) * 0.05;
                return r < bulgeradius ? getCentralIntensity(r, i0, k) : 
                                         getOuterIntensity(r - bulgeradius, getCentralIntensity(bulgeradius, i0, k), a);
            };

            for (int i = 0; i < width * height; i++)
            {
                var p0 = ArrayIndex.From1DTo2D(i, width);
                var distance = (p0 - new Vector2(width / 2, height / 2)).Length();
                var intensity = (float)Math.Clamp(getSurfaceBrightness(distance), 0, 1) * fn.GetPerlinFractal(p0.X * 5, p0.Y * 5);
                var color = new Rgba32(intensity, intensity, intensity);
                buffer[i] = color;
            }

            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");
            CreateDirectoryIfNotExists("./data");
            ImageWriter.FastWrite(ref buffer, $"./data/TestIntensityImage-{time}.png", width, height);
        }

        private static void Test1024x768Write()
        {
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.Write(ref buffer, $"./data/Test1024x768Write-{time}.png", width, height);
        }

        private static void TestFastWrite()
        {
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => new Rgba32((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            ImageWriter.FastWrite(ref buffer, $"./data/TestFastWrite-{time}.png", width, height);
        }

        private static void TestWrite()
        {
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            Measure(() => ImageWriter.Write(ref buffer, $"./data/TestWrite-{time}.png", width, height), true);
        }

        private static void Test8KFastWrite()
        {
            var rnd = new Random();
            var buffer = Enumerable.Repeat(0, width * height).Select(i => new Rgba32((byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))).ToArray();
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");

            CreateDirectoryIfNotExists("./data");
            Measure(() => ImageWriter.FastWrite(ref buffer, $"./data/Test8KFastWrite-{time}.png", width, height), true);
        }

        private static void TestCreateAndShowArray()
        {
            var rnd = new Random();
            Console.WriteLine($"Creating dataset {width}x{height} = {width * height} pixels");
            var a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 255)).ToArray();
            Console.WriteLine($"10 first values= [{string.Join(", ", a.Take(10))}]");
        }

        private static void TestAddMultiply()
        {
            int n = 5, runs = 5;
            var rnd = new Random();
            int[] result = new int[width*height];
            var a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 20)).ToArray();
            var b = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 20)).ToArray();
            
            Console.WriteLine("SIMD Addition/Multiplication");
            Console.WriteLine($"[{n}] first values of [a] = [{string.Join(", ", a.Take(n))}]");
            Console.WriteLine($"[{n}] first values of [b] = [{string.Join(", ", b.Take(n))}]");
            for (int x = 0; x < runs; x++)
            {
                var sw = Measure(() => SIMD.Add(ref a, ref b, ref result));
                Console.WriteLine($"[+] [{sw}] [{n}] first values of [result] = [{string.Join(", ", result.Take(n))}]");

                sw = Measure(() => SIMD.Multiply(ref a, ref b, ref result));
                Console.WriteLine($"[*] [{sw}] [{n}] first values of [result] = [{string.Join(", ", result.Take(n))}]");
            }

            Console.WriteLine();
            Console.WriteLine("NoSIMD Addition/Multiplication");
            Console.WriteLine($"[{n}] first values of [a] = [{string.Join(", ", a.Take(n))}]");
            Console.WriteLine($"[{n}] first values of [b] = [{string.Join(", ", b.Take(n))}]");
            for (int x = 0; x < runs; x++)
            {
                var sw = Measure(() => NoSIMD.Add(ref a, ref b, ref result));
                Console.WriteLine($"[+] [{sw}] [{n}] first values of [result] = [{string.Join(", ", result.Take(n))}]");

                sw = Measure(() => NoSIMD.Multiply(ref a, ref b, ref result));
                Console.WriteLine($"[*] [{sw}] [{n}] first values of [result] = [{string.Join(", ", result.Take(n))}]");
            }
        }
    }
}