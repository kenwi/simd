using System;
using System.Diagnostics;
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
        protected abstract void CreateResources();
        protected GameTime gameTime;
        
        private readonly FrameTimeAverager frameTimeAverager = new FrameTimeAverager(0.666);
        private TimeSpan TotalElapsedTime => gameTime?.TotalGameTime ?? TimeSpan.Zero;
        
        public Application(bool LimitFrameRate = false)
        {
            this.LimitFrameRate = LimitFrameRate;
        }
        
        public void Run()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();
            CreateResources();

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            while(IsRunning)
            {
                gameTime = new GameTime(TotalElapsedTime + stopWatch.Elapsed, stopWatch.Elapsed);
                var dt = gameTime.ElapsedGameTime.TotalSeconds;
                stopWatch.Restart();

                while(LimitFrameRate && dt < DesiredFrameLengthSeconds)
                {
                    var elapsed = stopWatch.Elapsed;
                    gameTime = new GameTime(TotalElapsedTime + elapsed, gameTime.ElapsedGameTime + elapsed);
                    dt += elapsed.TotalSeconds;
                    stopWatch.Restart();
                }

                if(dt > DesiredFrameLengthSeconds * 1.25)
                    gameTime = GameTime.RunningSlowly(gameTime);

                frameTimeAverager.AddTime(dt);

                Update(dt);
                if (IsRunning)
                    Render(dt);
            }
        }

        protected virtual void Render(double dt)
        {
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
