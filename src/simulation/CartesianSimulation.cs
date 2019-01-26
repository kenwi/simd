using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    public class CartesianSimulation
    {
        int width = 800, height = 600;
        int numParticles = 10000;
        CartesianSimulationComputation computation;
        public float[] px, py;
        public float[] ax, ay;
        public float[] vx, vy;
        Rgba32[] canvas;

        public CartesianSimulation(int numParticles)
        {
            this.numParticles = numParticles;
            Setup();
        }

        [GlobalSetup]
        private void Setup()
        {
            var rnd = new Random();
            computation = new CartesianSimulationComputation(numParticles);
            px = Enumerable.Repeat(0, numParticles).Select(i => (float)Math.Clamp(rnd.NextDouble(), -1, 1) * width).ToArray();
            py = Enumerable.Repeat(0, numParticles).Select(i => (float)Math.Clamp(rnd.NextDouble(), -1, 1) * height).ToArray();
            ax = new float[numParticles];
            ay = Enumerable.Repeat(0, numParticles).Select(i => (float)rnd.NextDouble()).ToArray();
            vx = new float[numParticles];
            vy = new float[numParticles];
        }

        public void Step(float dt, int numSteps)
        {
            for(int i=0; i<numSteps; i++)
            {
                Console.WriteLine($"Step [{i}/{numSteps}]");
                canvas = new Rgba32[width * height];
                for(int j=0; j<canvas.Length; j++)
                    canvas[j].A = 255;

                computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);

                UpdateCanvas(ref canvas, ref px, ref py);
                ImageWriter.FastWrite(ref canvas, $"./data/Simulation-{i}.png", width, height);
            }
        }

        public void UpdateCanvas(ref Rgba32[] canvas, ref float[] positionX, ref float[] positionY)
        {
            for(int i=0; i<numParticles; i++)
            {
                var x = (int)positionX[i];
                var y = (int)positionY[i];
                if(x < width && y < height)
                {
                    var index = ArrayIndex.From2DTo1D(x, y, width);
                    canvas[index] = new Rgba32(255, 255, 255, 255);
                }
            }
        }
    }
}