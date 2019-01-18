using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Engines;

namespace simd
{
    [SimpleJob()]
    public class SimdBenchmark
    {
        Random rnd;
        int width;
        int height;
        int[] a, b, c;

        [GlobalSetup]
        public void Setup()
        {
            width = 1920;
            height = 1080;
            rnd = new Random();
            a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(1, 1000)).ToArray();
            b = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(1, 1000)).ToArray();
            c = new int[width * height];
        }
        [Benchmark] public void AddNoSimd() => NoSIMD.Add(ref a, ref b, ref c);
        [Benchmark] public void MultiplyNoSimd() => NoSIMD.Multiply(ref a, ref b, ref c);
        [Benchmark] public void Add() => SIMD.Add(ref a, ref b, ref c);
        [Benchmark] public void Multiply() => SIMD.Multiply(ref a, ref b, ref c);
        [Benchmark] public void ExecuteAdd() => SIMD.ExecuteOnSets(ref a, ref b, ref c, (a, b) => a + b);
        [Benchmark] public void ExecuteMultiply() => SIMD.ExecuteOnSets(ref a, ref b, ref c, (a, b) => a * b);
    }
}