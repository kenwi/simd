using System;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    partial class Program
    {
        static void Main(string[] args)
        {
            if (!Vector.IsHardwareAccelerated)
                throw new Exception("No hw acceleration available.");

            int width = 1024, height = 768;
            Vector2 center = new Vector2(width / 2, height / 2);
            
            var frameBuffer = new FrameBuffer(width, height);
            frameBuffer.MakeTestBuffer();
            frameBuffer.Write("./data/framebuffer.png");
            frameBuffer.SetPixels((x, y) => new Rgba32(x/(float)width, y/(float)height, 0.5f));
            frameBuffer.Write("./data/framebuffer2.png");


            /*int width = 1920, 
                height = 1080,
                timeSteps = 500,
                numParticles = 1000000;
            float deltaTime = 0.00001f;
            
            var particles = new Particles(numParticles);
            var simulation = new GalaxySimulationComputation(ref particles, width, height);
            simulation.InitializeParticles(width/30, height/30);
            simulation.Run(deltaTime, timeSteps);*/
        }
    }
}
