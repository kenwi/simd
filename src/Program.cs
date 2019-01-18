﻿using System;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace simd
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            //var config = new BenchmarkDotNet.Configs.DebugInProcessConfig();
            var result = BenchmarkRunner.Run<SimdBenchmark>();

            //Console.WriteLine($"Run Time = {Measure(TestGenerateBasicMap, true)}"+ Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestCreateAndShowArray, true)}"+ Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestAddMultiply, true)}"+ Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestExecuteOnSetsMethod, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestIntensityImage, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestFastWrite, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(TestWrite, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(Test8KFastWrite, true)}" + Environment.NewLine);
            //Console.WriteLine($"Run Time = {Measure(Test1024x768Write, true)}" + Environment.NewLine);
        }
    }
}