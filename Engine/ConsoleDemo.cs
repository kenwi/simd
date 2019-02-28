using System;
using System.Diagnostics;
using Veldrid;

namespace DemoApplication
{
    public class ConsoleDemo : Engine.Application
    {
        public static ConsoleDemo Instance => _instance;
        static readonly ConsoleDemo _instance = new ConsoleDemo();
        uint numFrames = 0;

        public ConsoleDemo()
        {
            this.LimitFrameRate = true;
            this.DesiredUpdateRate = 1;
            this.DesiredFrameRate = 1;
        }
        
        protected override GraphicsDevice CreateGraphicsDevice()
        {
            Console.WriteLine($"[{DateTime.Now}] Creating graphics device");
            return null;
        }

        protected override void CreateResources()
        {
            Console.WriteLine($"[{DateTime.Now}] Creating resources");
        }

        protected override void GetUserInput()
        {
        }

        protected override void Update(double dt)
        {
            Console.WriteLine($"[{DateTime.Now}] Update: {dt}, Iteration: {numFrames++}");
        }
    }
}