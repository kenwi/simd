using System;
using System.ComponentModel;
using System.Diagnostics;
using Veldrid;
using Thread = System.Threading.Thread;

namespace DemoApplication
{
    public class ConsoleDemo : Engine.Application
    {
        public static ConsoleDemo Instance => _instance;
        static readonly ConsoleDemo _instance = new ConsoleDemo();

        Stopwatch stopwatch = new Stopwatch();
        BackgroundWorker inputBackgroundWorker = new BackgroundWorker();
        uint numFrames = 0, numUpdates = 0;

        public ConsoleDemo()
        {
            this.LimitFrameRate = true;
            this.inputBackgroundWorker.DoWork += (s, e) => GetEvents();
            this.inputBackgroundWorker.RunWorkerAsync();
        }

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            Console.WriteLine($"[{DateTime.Now}] Creating graphics device");
            return null;
        }

        protected override void CreateResources()
        {
            Console.WriteLine($"[{DateTime.Now}] Creating resources");
            Console.WriteLine($"[{DateTime.Now}] IsInputRedirected {Console.IsInputRedirected}");
            stopwatch.Start();
        }

        protected override void GetEvents()
        {
            if (!Console.IsInputRedirected && Console.KeyAvailable)
            {
                var input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.Q:
                        Exit();
                        break;

                    default:
                        Console.WriteLine($"[{DateTime.Now}] Key [{input.Key}]");
                        break;
                }
            }
        }

        protected override void Render(double dt)
        {
            numFrames++;
            if(stopwatch.Elapsed.TotalSeconds > 2)
            {
                Console.WriteLine($"[{DateTime.Now}] [Render] Dt: {dt}, Frames: {numFrames}, Updates: {numUpdates}");
                stopwatch.Restart();
            }
        }

        protected override void Update(double dt)
        {
            numUpdates++;
            if(stopwatch.Elapsed.TotalSeconds > 2)
            {
                Console.WriteLine($"[{DateTime.Now}] [Update] Dt: {dt}, Frames: {numFrames}, Updates: {numUpdates}");
                stopwatch.Restart();
            }
        }
    }
}