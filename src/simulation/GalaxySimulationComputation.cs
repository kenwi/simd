using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    public class GalaxySimulationComputation
    {
        Rgba32[] blankCanvas;
        Rgba32[] canvas;
        Particles particles;

        int numParticles;
        int width,
            height;

        Vector2 canvasCenter;

        public GalaxySimulationComputation(ref Particles particles, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.particles = particles;
            this.numParticles = particles.numParticles;
            canvas = Enumerable.Repeat<Rgba32>(new Rgba32(0, 0, 0), width * height).ToArray();
            canvasCenter = new Vector2(width / 2, height / 2);
            blankCanvas = new Rgba32[width * height];
            canvas.CopyTo(blankCanvas, 0);
        }

        public void InitializeParticles(int width, int height)
        {
            var rnd = new Random();
            for (int i = 0; i < numParticles; i++)
            {
                particles.positionX[i] = canvasCenter.X + (float)(Math.Clamp(rnd.NextDouble(), -1.0, 1.0) * width);
                particles.positionY[i] = canvasCenter.Y + (float)(Math.Clamp(rnd.NextDouble(), -1.0, 1.0) * height);

                float d = new Vector2(canvasCenter.X + particles.positionX[i], canvasCenter.Y + particles.positionY[i]).Length() * 1000;
                particles.vTheta[i] = (float)Orbit.CalculateOrbitalVelocity(d);
                particles.theta[i] = 360 * (float)rnd.NextDouble();
                particles.a[i] = 10;
                particles.b[i] = -10;
                particles.centerX[i] = width / 2;
                particles.centerY[i] = width / 2;
            }
        }

        internal void Run(float deltaTime, int timeSteps)
        {
            for (int i = 0; i < timeSteps; i++)
            {
                Step(deltaTime, i);
                UpdateCanvas(ref canvas, ref particles.velocityX, ref particles.velocityY, ref particles.positionX, ref particles.positionY);
                ImageWriter.FastWrite(ref canvas, $"F:/data/Simulation-{i}.png", width, height);
                Console.WriteLine($"[{DateTime.Now.ToLocalTime()}] [{i}/{timeSteps}]");
            }
            Console.WriteLine($"Done");
        }

        public void Step(float dt, int fileIndex)
        {
            float oldX, oldY;
            for (int i = 0; i < numParticles; i++)
            {
                particles.theta[i] += particles.vTheta[i] * dt;
                oldX = particles.positionX[i];
                oldY = particles.positionY[i];

                Orbit.Calculate(particles.angle[i],
                                particles.a[i],
                                particles.b[i],
                                particles.theta[i],
                                ref particles.positionX[i], ref particles.positionY[i]);

                particles.velocityX[i] = particles.positionX[i] - oldX;
                particles.velocityY[i] = particles.positionY[i] - oldY;
            }

        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float wrapAround(float coordinate, float max)
        {
            coordinate %= max + 1;
            if (coordinate < 0)
                coordinate += max;
            return coordinate;
        }

        public void UpdateCanvas(ref Rgba32[] canvas, ref float[] vX, ref float[] vY, ref float[] positionX, ref float[] positionY)
        {
            blankCanvas.CopyTo(canvas, 0);
            for (int i = 0; i < numParticles; i++)
            {
                //var px = positionX[i] * (width / sceneSize.X);
                //var py = positionY[i] * (height / sceneSize.X);
                var px  = wrapAround(positionX[i], width - 1);
                var py = wrapAround(positionY[i], height - 1);

                var index = ArrayIndex.From2DTo1D((int)px, (int)py, width);
                canvas[index] = new Rgba32(255, 255, 255);

                positionX[i] = px;
                positionY[i] = py;
            }
        }
    }
}