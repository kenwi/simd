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

        DateTime startTime = DateTime.Now;
        Stopwatch renderStopwatch = new Stopwatch(), updateStopwatch = new Stopwatch();
        uint numFrames = 0, numUpdates = 0;

        static void PrintLine(string output) => Console.WriteLine($"[{DateTime.Now}] {output}");

        public ConsoleDemo()
        {
            LimitFrameRate = true;
            TargetUpdateRate = 60.0;
        }

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            PrintLine("Creating graphics device");
            return null;
        }

        protected override void CreateResources()
        {
            PrintLine("Creating resources");
            PrintLine($"IsInputRedirected {Console.IsInputRedirected}");
            PrintLine($"LimitFrameRate {LimitFrameRate}");
            renderStopwatch.Start();
            updateStopwatch.Start();
        }

        protected override void GetEvents()
        {
            if (!Console.IsInputRedirected && Console.KeyAvailable)
            {
                var input = Console.ReadKey(true);
                switch (input.Key)
                {
                    case ConsoleKey.Q:
                        PrintLine($"Key [{input.Key}]");
                        Exit();
                        break;

                    default:
                        PrintLine($"Key [{input.Key}]");
                        break;
                }
            }
        }

        protected override void Render(double dt)
        {
            numFrames++;
            if (renderStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats(dt, numFrames, numUpdates, startTime);
                renderStopwatch.Restart();
            }
        }

        protected override void Update(double dt)
        {
            numUpdates++;
            if (updateStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats(dt, numFrames, numUpdates, startTime);
                updateStopwatch.Restart();
            }
        }

        private void printStats(double dt, uint numFrames, uint numUpdates, DateTime startTime)
        {
            var fps = numFrames / (DateTime.Now - startTime).TotalSeconds;
            var ups = numUpdates / (DateTime.Now - startTime).TotalSeconds;
            PrintLine($"[Render] Dt: {dt:0.####} Frames: {numFrames} @ {fps:N} hz Updates: {numUpdates} @ {ups:N} hz");
        }
    }
}