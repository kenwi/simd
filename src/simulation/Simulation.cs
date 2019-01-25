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
        int width = 1024, height = 1024;
        int numParticles = 16;
        SimulationComputation computation;
        public float[] px, py;
        public float[] ax, ay;
        public float[] vx, vy;
        Rgba32[] canvas;

        public Simulation()
        {
            Setup();
        }

        [GlobalSetup]
        public void Setup()
        {
            computation = new SimulationComputation(numParticles);
            var rnd = new Random();
            px = Enumerable.Repeat(0, numParticles).Select(i => (float)rnd.NextDouble()).ToArray();
            py = new float[numParticles];
            ax = new float[numParticles];
            ay = Enumerable.Repeat(0, numParticles).Select(i => (float)rnd.NextDouble()).ToArray();
            vx = new float[numParticles];
            vy = Enumerable.Repeat(0, numParticles).Select(i => (float)rnd.NextDouble()).ToArray();
            canvas = new Rgba32[width * height];
            var time = DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture).Replace(":", "-");
            ImageWriter.FastWrite(ref canvas, $"./data/TestFastWrite-{time}.png", width, height);
        }
        public void Step(float dt)
        {            
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
            computation.Simulate(dt, ref px, ref py, ref vx, ref vy, ref ax, ref ay);
        }
    }
}