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

        public double DesiredUpdateRate;
        public double DesiredUpdateLengthSeconds => 1.0 / DesiredUpdateRate;
        
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

        public Application(bool LimitRate = true, double DesiredUpdateRate = 60.0)
        {
            this.DesiredUpdateRate = DesiredUpdateRate;
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
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                var dt = gameTime.ElapsedGameTime.TotalSeconds;

                while (LimitFrameRate && dt < DesiredUpdateLengthSeconds)
                {
                    gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, gameTime.ElapsedGameTime + stopWatch.Elapsed);
                    dt += stopWatch.Elapsed.TotalSeconds;
                    stopWatch.Restart();
                }

                if (dt > DesiredUpdateLengthSeconds * 1.25)
                    gameTime = GameTime.RunningSlowly(gameTime);

                GetEvents();
                Update(dt);

                if (IsRunning)
                {
                    Render(dt);
                    frameTimeAverager.AddTime(dt);
                    stopWatch.Restart();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunVariableUpdate()
        {
            double lag = 0;
            while (IsRunning)
            {
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                lag += gameTime.ElapsedGameTime.TotalSeconds;

                while (lag >= DesiredUpdateLengthSeconds)
                {
                    GetEvents();
                    Update(DesiredUpdateLengthSeconds);
                    lag -= DesiredUpdateLengthSeconds;
                }

                if (IsRunning)
                {
                    Render(lag / DesiredUpdateLengthSeconds);
                    frameTimeAverager.AddTime(gameTime.ElapsedGameTime.TotalSeconds);
                    stopWatch.Restart();
                }
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
