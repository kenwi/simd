using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    public class SimulationComputation
    {
        int numParticles;
        public Vector<float> px, py;
        public Vector<float> ax, ay;
        public Vector<float> vx, vy;

        public SimulationComputation(int numParticles)
        {
            this.numParticles = numParticles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Simulate(float dt, ref float[] PX, ref float[] PY, ref float[] VX, ref float[] VY, ref float[] AX, ref float[] AY)
        {
            int i, vecSize = Vector<float>.Count;
            for(i=0; i <= numParticles - vecSize; i += vecSize)
            {
                ax = new Vector<float>(AX, i);
                ay = new Vector<float>(AY, i);
                vx = new Vector<float>(VX, i);
                vy = new Vector<float>(VY, i);
                px = new Vector<float>(PX, i);
                py = new Vector<float>(PY, i);

                vx = vx + (ax * dt);
                vy = vy + (ay * dt);
                px = px + vx;
                py = py + vy;
                ax.CopyTo(AX, i);
                ay.CopyTo(AY, i);
                vx.CopyTo(VX, i);
                vy.CopyTo(VY, i);
                px.CopyTo(PX, i);
                py.CopyTo(PY, i);
            }
            for(; i<numParticles; i++)
            {
                AX[i] = ax[i] * dt;
                AY[i] = ay[i] * dt;
                VX[i] = vx[i] + ax[i] * dt;
                VY[i] = vy[i] + ay[i] * dt;
                PX[i] = px[i] + vx[i];
                PY[i] = py[i] + vy[i];
            }
        } 
    }

    public class Simulation
    {
        int width = 800, height = 600;
        int numParticles = 10000;
        SimulationComputation computation;
        public float[] px, py;
        public float[] ax, ay;
        public float[] vx, vy;
        Rgba32[] canvas;

        public Simulation(int numParticles)
        {
            this.numParticles = numParticles;
            Setup();
        }

        [GlobalSetup]
        private void Setup()
        {
            var rnd = new Random();
            computation = new SimulationComputation(numParticles);
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