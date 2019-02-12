using System;
using System.Diagnostics;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp.PixelFormats;
using static FastNoise;

namespace simd
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            int width = 1920,
                height = 1280;
            float scale = 1.0f;
            var frameBuffer = new FrameBuffer(width, height);
            var fastnoise = new FastNoise();
            fastnoise.SetNoiseType(NoiseType.PerlinFractal);
            fastnoise.SetFractalGain(0.5f);
            fastnoise.SetFractalOctaves(4);
            fastnoise.SetFrequency(0.01f);
            Vector2 position = new Vector2(0, 0);
            frameBuffer.SetPixels((x, y) =>
            {
                var h = fastnoise.GetNoise(position.X + x * scale, position.Y + y * scale);
                return new Rgba32(0.4f + h, 0.4f + h, 0.4f + h);
            });
            frameBuffer.Write("./data/noisebuffer.png");

            frameBuffer.MakeTestBuffer();
            frameBuffer.Write("./data/framebuffer1.png");

            frameBuffer.SetPixels((x, y) => new Rgba32(x / (float)width, y / (float)height, 0.5f));
            frameBuffer.Write("./data/framebuffer2.png");

            int numParticles = 10000, timeSteps = 1;
            float deltaTime = 0.1f;
            var particles = new Particles(numParticles);
            var simulation = new GalaxySimulationComputation(ref particles, width, height);
            simulation.InitializeParticles(width, height);
            simulation.Run(deltaTime, timeSteps);
            Console.WriteLine("Done");
        }
    }
}
