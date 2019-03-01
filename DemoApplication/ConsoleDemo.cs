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

        Stopwatch renderStopwatch = new Stopwatch(), updateStopwatch = new Stopwatch();
        DateTime startTime = DateTime.Now;
        uint frameCount = 0, updateCount = 0;

        static void PrintLine(string output) => Console.WriteLine($"[{DateTime.Now}] {output}");

        public ConsoleDemo()
        {
            LimitFrameRate = false;
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
            frameCount++;
            if (renderStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats("Render", dt, frameCount, updateCount, startTime);
                renderStopwatch.Restart();
            }
        }

        protected override void Update(double dt)
        {
            updateCount++;
            if (updateStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats("Update", dt, frameCount, updateCount, startTime);
                updateStopwatch.Restart();
            }
        }

        private void printStats(string label, double dt, uint numFrames, uint numUpdates, DateTime startTime)
        {
            var fps = numFrames / (DateTime.Now - startTime).TotalSeconds;
            var ups = numUpdates / (DateTime.Now - startTime).TotalSeconds;
            PrintLine($"[{label}] Dt: {dt:0.####} Frames: {numFrames} @ {fps:N} hz Updates: {numUpdates} @ {ups:N} hz");
        }
    }
}