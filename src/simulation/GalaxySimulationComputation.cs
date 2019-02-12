using System;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    public class GalaxySimulationComputation
    {
        Particles particles;
        FrameBuffer frameBuffer;
        int[] indices;

        int numParticles;
        Vector2 sceneSize;

        public string DataDirectory { get; internal set; }

        public GalaxySimulationComputation(ref Particles particles, int width, int height, string dataDirectory = "./data")
        {
            this.DataDirectory = dataDirectory;
            this.particles = particles;
            this.numParticles = particles.numParticles;
            this.indices = new int[numParticles];
            this.frameBuffer = new FrameBuffer(width, height);
        }

        public void InitializeParticles(float sceneWidth, int sceneHeight)
        {
            sceneSize = new Vector2(sceneWidth, sceneHeight);
            var rnd = new Random();

            Func<int, int, int, int> getIndex = (b, s, x) => b + s *x;
            for (int i = 0; i < numParticles; i++)
            {
                indices[i] = getIndex(2, 1, i);
                /*particles.positionX[i] = frameBuffer.center.X + (float)(Math.Clamp(rnd.NextDouble(), -1.0, 1.0) * sceneSize.X);
                particles.positionY[i] = frameBuffer.center.Y + (float)(Math.Clamp(rnd.NextDouble(), -1.0, 1.0) * sceneSize.Y);

                float d = new Vector2(frameBuffer.center.X + particles.positionX[i], frameBuffer.center.Y + particles.positionY[i]).Length();
                particles.vTheta[i] = (float)Orbit.CalculateOrbitalVelocity(d);
                particles.theta[i] = 360 * (float)rnd.NextDouble();
                particles.a[i] = 10;
                particles.b[i] = -10;
                particles.centerX[i] = sceneSize.X / 2;
                particles.centerY[i] = sceneSize.Y / 2;*/
            }
            ;
        }

        internal void Run(float deltaTime, int timeSteps)
        {
            for (int i = 0; i < timeSteps; i++)
            {
                Step(deltaTime, i);
                frameBuffer.Update(ref particles.positionX, ref particles.positionY);
                ImageWriter.FastWrite(frameBuffer.GetFrameBuffer(), $"{DataDirectory}/Simulation-{i}.png", frameBuffer.width, frameBuffer.height);
                Console.WriteLine($"[{DateTime.Now.ToLocalTime()}] [{i}/{timeSteps}]");
            }
            Console.WriteLine($"Done");
        }

        public void Step(float dt, int fileIndex)
        {
            float scaleX = sceneSize.X / frameBuffer.width;
            float scaleY = sceneSize.Y / frameBuffer.height;
            float oldX, oldY;
            for (int i = 0; i < numParticles; i++)
            {
                particles.theta[i] += particles.vTheta[i] * dt;
                oldX = particles.positionX[i] * scaleX;
                oldY = particles.positionY[i] * scaleY;

                /*Orbit.Calculate(particles.angle[i],
                                particles.a[i],
                                particles.b[i],
                                particles.theta[i],
                                ref particles.positionX[i], ref particles.positionY[i]);
                */
                particles.velocityX[i] = particles.positionX[i] * scaleX - oldX;
                particles.velocityY[i] = particles.positionY[i] * scaleY - oldY;
            }
        }
    }
}