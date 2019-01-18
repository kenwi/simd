using System;
using System.Linq;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;


namespace simd 
{
    public class SimdBenchmark
    {
        Random rnd;
        int width;
        int height;
        int[] a, b, c;

        [GlobalSetup]
        public void Setup()
        {
            width = 1024;
            height = 768;
            rnd = new Random();
            a = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 999)).ToArray();
            b = Enumerable.Repeat(0, width * height).Select(i => rnd.Next(0, 999)).ToArray();
            c = new int[width * height];
        }

        [Benchmark]
        public int[] Add()
        {
            SIMD.Add(ref a, ref b, ref c);
            return c;
        }

        [Benchmark]
        public int[] AddNoSimd()
        {
            NoSIMD.Add(ref a, ref b, ref c);
            return c;
        }
    }
}