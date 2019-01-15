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
            
            Console.WriteLine($"Run Time = {Measure(TestFastWrite, true)}"+ Environment.NewLine);
            Console.WriteLine($"Run Time = {Measure(Test8KFastWrite, true)}"+ Environment.NewLine);
            Console.WriteLine($"Run Time = {Measure(Test1024x768Write, true)}" + Environment.NewLine);
            Console.WriteLine($"Run Time = {Measure(Test8KWrite, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestCreateAndShowArray, true)}"+ Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(Test8KAddMultiply, true)}"+ Environment.NewLine);
        }
    }
}
