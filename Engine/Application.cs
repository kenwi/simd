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
        protected abstract void Update(float dt);
        protected abstract void CreateResources();

        public void Run()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();
            CreateResources();

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
