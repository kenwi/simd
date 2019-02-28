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
        public double DesiredFrameRate;
        public double DesiredFrameLengthSeconds => 1.0 / DesiredFrameRate;
        public double DesiredUpdateLengthSeconds => 1.0 / DesiredUpdateRate;
        
        public double FramesPerSecond => Math.Round(frameTimeAverager.CurrentAverageFramesPerSecond);
        public int TotalFrames => frameTimeAverager.TotalFrames;

        protected abstract GraphicsDevice CreateGraphicsDevice();
        protected abstract void Update(double dt);
        protected abstract void GetUserInput();
        protected abstract void CreateResources();
        protected GameTime gameTime;

        private Stopwatch stopWatch = new Stopwatch();
        private readonly FrameTimeAverager frameTimeAverager = new FrameTimeAverager();
        private TimeSpan TotalElapsedTime => gameTime?.TotalGameTime ?? TimeSpan.Zero;

        public Application(bool LimitRate = false, double DesiredFrameRate = 60.0, double DesiredUpdateRate = 60.0)
        {
            this.DesiredUpdateRate = DesiredUpdateRate;
            this.DesiredFrameRate = DesiredFrameRate;
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
                RunUpdateLimited();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunRateLimited()
        {
            while (IsRunning)
            {
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                var dt = gameTime.ElapsedGameTime.TotalSeconds;
                stopWatch.Restart();

                GetUserInput();
                while (LimitFrameRate && dt < DesiredFrameLengthSeconds)
                {
                    var elapsed = stopWatch.Elapsed;
                    gameTime = new GameTime(TotalElapsedTime + elapsed, gameTime.ElapsedGameTime + elapsed);
                    dt += elapsed.TotalSeconds;
                    stopWatch.Restart();
                }

                if (dt > DesiredFrameLengthSeconds * 1.25)
                    gameTime = GameTime.RunningSlowly(gameTime);

                frameTimeAverager.AddTime(dt);
                Update(dt);

                if (IsRunning)
                    Render();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunUpdateLimited()
        {
            double lag = 0;
            while (IsRunning)
            {
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                lag += gameTime.ElapsedGameTime.TotalMilliseconds;
                frameTimeAverager.AddTime(gameTime.ElapsedGameTime.TotalMilliseconds);

                GetUserInput();
                while (lag >= DesiredUpdateLengthSeconds)
                {
                    Update(DesiredUpdateLengthSeconds);
                    lag -= DesiredUpdateLengthSeconds;
                }

                if (IsRunning)
                {
                    Render();
                    stopWatch.Restart();
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Render()
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
