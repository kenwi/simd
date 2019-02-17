using System;
using System.Diagnostics;
using Veldrid;

namespace Engine
{
    public abstract class Application : IDisposable
    {
        public bool IsRunning { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Framebuffer FrameBuffer => GraphicsDevice.SwapchainFramebuffer;
        
        protected abstract GraphicsDevice CreateGraphicsDevice();

        public void Run()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();

            const float dt = 0.1f;
            while(IsRunning)
            {
                Update(dt);
                if (IsRunning)
                    Render(dt);
            }
        }

        protected virtual void Render(float dt)
        {
            GraphicsDevice.SwapBuffers();
            GraphicsDevice.WaitForIdle();
        }

        protected virtual void Update(float dt)
        {
            
        }

        public void Exit()
        {
            Console.WriteLine("Exiting");
            IsRunning = false;
        }

        public virtual void Dispose()
        {
            Console.WriteLine("Disposing");
            GraphicsDevice.Dispose();
        }
    }
}
