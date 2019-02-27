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
        public double DesiredFrameLengthSeconds => 1.0 / 60.0;
        public double FramesPerSecond => Math.Round(frameTimeAverager.CurrentAverageFramesPerSecond);
        public int TotalFrames => frameTimeAverager.TotalFrames;

        protected abstract GraphicsDevice CreateGraphicsDevice();
        protected abstract void Update(double dt);
        protected abstract void GetUserInput();
        protected abstract void CreateResources();
        protected GameTime gameTime;

        private readonly FrameTimeAverager frameTimeAverager = new FrameTimeAverager(0.666);
        private TimeSpan TotalElapsedTime => gameTime?.TotalGameTime ?? TimeSpan.Zero;

        public Application(bool LimitFrameRate = true)
        {
            this.LimitFrameRate = LimitFrameRate;
        }

        public void Run()
        {
            if (LimitFrameRate)
                RunFrameRateLimited();
            else
                RunVariableTimeStep();
        }

        private void RunVariableTimeStep()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();
            CreateResources();
            var stopWatch = new Stopwatch();

            int updatesPerSecond = 100;
            double lag = 0, msPerUpdate = 1.0 / updatesPerSecond;
            while (IsRunning)
            {
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                lag += gameTime.ElapsedGameTime.TotalMilliseconds;
                frameTimeAverager.AddTime(gameTime.ElapsedGameTime.TotalMilliseconds);

                GetUserInput();
                while (lag >= msPerUpdate)
                {
                    Update(gameTime.ElapsedGameTime.TotalMilliseconds);
                    lag -= msPerUpdate;
                }
                Render();
                stopWatch.Restart();
            }
        }

        private void RunFrameRateLimited()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();
            CreateResources();
            var stopWatch = new Stopwatch();

            gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
            stopWatch.Restart();

            while (IsRunning)
            {
                GetUserInput();
                Update(gameTime.ElapsedGameTime.TotalSeconds);

                while (LimitFrameRate && gameTime.ElapsedGameTime.TotalSeconds < DesiredFrameLengthSeconds)
                {
                    var elapsed = stopWatch.Elapsed;
                    gameTime = new GameTime(TotalElapsedTime + elapsed, gameTime.ElapsedGameTime + elapsed);
                    stopWatch.Restart();
                }

                if (gameTime.ElapsedGameTime.TotalSeconds > DesiredFrameLengthSeconds * 1.25)
                    gameTime = GameTime.RunningSlowly(gameTime);

                frameTimeAverager.AddTime(gameTime.ElapsedGameTime.TotalSeconds);
                Render();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Render()
        {
            if (!IsRunning)
                return;
            GraphicsDevice.SwapBuffers();
            GraphicsDevice.WaitForIdle();
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
