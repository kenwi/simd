using System;
using System.Diagnostics;
using Veldrid;

namespace Engine
{
    public abstract class Application
    {
        public bool IsRunning { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        protected abstract GraphicsDevice CreateGraphicsDevice();

        public void Run()
        {
            IsRunning = true;
            GraphicsDevice = CreateGraphicsDevice();

            const float dt = 0.1f;
            while(IsRunning)
            {
                Update(dt);
                Render(dt);
            }
        }

        private void Render(float dt)
        {
            GraphicsDevice.SwapBuffers();
            GraphicsDevice.WaitForIdle();
        }

        private void Update(float dt)
        {

        }
    }
}
