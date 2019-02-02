using System;
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

            int width = 1920, height = 1080;
            var deltaTime = 0.00001f;
            var timeSteps = 500;
            var particles = new Particles(1000000);
            var simulation = new GalaxySimulationComputation(ref particles, width, height);
            simulation.InitializeParticles(width/30, height/30);
            simulation.Run(deltaTime, timeSteps);
        }
    }
}
