using System;
using System.Numerics;

namespace heightmap_simd
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            Console.WriteLine($"Run Time = {Measure(TestWrite, true)}" + Environment.NewLine);
            Console.WriteLine($"Run Time = {Measure(TestCreateAndShowArray, true)}"+ Environment.NewLine);
            Console.WriteLine($"Run Time = {Measure(Test8KAddMultiply, true)}"+ Environment.NewLine);
        }
    }
}
