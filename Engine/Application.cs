using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Veldrid;

namespace Engine
{
    public abstract class Application : IDisposable
    {
        public bool IsRunning { get; private set; }
        public bool LimitFrameRate { get; protected set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Framebuffer FrameBuffer => GraphicsDevice.SwapchainFramebuffer;

        public double TargetUpdateRate;
        public double TargetUpdateLengthSeconds => 1.0 / TargetUpdateRate;

        public double FramesPerSecond => Math.Round(frameTimeAverager.CurrentAverageFramesPerSecond);
        public int TotalFrames => frameTimeAverager.TotalFrames;

        protected abstract GraphicsDevice CreateGraphicsDevice();
        protected abstract void Update(double dt);
        protected abstract void GetEvents();
        protected abstract void CreateResources();
        protected GameTime gameTime;

        private Stopwatch stopWatch = new Stopwatch();
        private readonly FrameTimeAverager frameTimeAverager = new FrameTimeAverager();
        private TimeSpan TotalElapsedTime => gameTime?.TotalGameTime ?? TimeSpan.Zero;

        DateTime previousTime;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double calculateDt()
        {
            var currentTime = DateTime.Now;
            var dt = currentTime - previousTime;
            previousTime = currentTime;
            return dt.TotalSeconds;
        }

        public Application(bool LimitRate = true, double TargetUpdateRate = 60.0)
        {
            this.TargetUpdateRate = TargetUpdateRate;
            this.LimitFrameRate = LimitRate;
            stopWatch.Start();
        }

        public void Run()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();
            CreateResources();

            if (LimitFrameRate)
                RunRateLimited();
            else
                RunVariableUpdate();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunRateLimited()
        {
            while (IsRunning)
            {
                var dt = calculateDt();
                while (LimitFrameRate && dt < TargetUpdateLengthSeconds)
                    dt += calculateDt();

                GetEvents();
                Update(dt);

                if (IsRunning)
                    Render(dt);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunVariableUpdate()
        {
            double lag = 0;
            previousTime = DateTime.Now;
            while (IsRunning)
            {
                var dt = calculateDt();
                lag += dt;

                while (lag >= TargetUpdateLengthSeconds)
                {
                    GetEvents();
                    Update(TargetUpdateLengthSeconds);
                    lag -= TargetUpdateLengthSeconds;
                }

                if (IsRunning)
                    Render(lag / TargetUpdateLengthSeconds);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Render(double dt)
        {
            GraphicsDevice?.SwapBuffers();
            GraphicsDevice?.WaitForIdle();
        }

        public void Exit()
        {
            Console.WriteLine("Exiting");
            IsRunning = false;

        }

        public virtual void Dispose()
        {
            Console.WriteLine("Disposing GraphicsDevice");
            GraphicsDevice.Dispose();
        }
    }
}
