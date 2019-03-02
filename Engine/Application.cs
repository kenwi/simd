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

        public double TargetUpdatesPerSecond;
        public double TargetUpdateRate => 1.0 / TargetUpdatesPerSecond;

        protected abstract GraphicsDevice CreateGraphicsDevice();
        protected abstract void CreateResources();
        protected abstract void GetEvents();
        protected abstract void Update(double dt);
        protected abstract void Render(double dt);

        DateTime previousTime;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double calculateDt()
        {
            var currentTime = DateTime.Now;
            var dt = currentTime - previousTime;
            previousTime = currentTime;
            return dt.TotalSeconds;
        }

        public Application(bool LimitRate = true, double TargetUpdatesPerSecond = 60.0)
        {
            this.TargetUpdatesPerSecond = TargetUpdatesPerSecond;
            this.LimitFrameRate = LimitRate;
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
                while (LimitFrameRate && dt < TargetUpdateRate)
                    dt += calculateDt();

                GetEvents();
                Update(dt);

                if (IsRunning)
                {
                    Render(dt);
                    GraphicsDevice?.SwapBuffers();
                    GraphicsDevice?.WaitForIdle();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunVariableUpdate()
        {
            double lag = 0;
            previousTime = DateTime.Now;
            while (IsRunning)
            {
                lag += calculateDt();
                while (IsRunning && lag >= TargetUpdateRate)
                {
                    GetEvents();
                    Update(TargetUpdateRate);
                    lag -= TargetUpdateRate;
                }

                if (IsRunning)
                {
                    Render(lag / TargetUpdateRate);
                    GraphicsDevice?.SwapBuffers();
                    GraphicsDevice?.WaitForIdle();
                }
            }
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
