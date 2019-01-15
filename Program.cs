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

            Console.WriteLine($"Run Time = {Measure(Run8KImageTest, true)}");
        }

    }
}
