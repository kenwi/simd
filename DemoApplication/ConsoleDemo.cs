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

        protected override GraphicsDevice CreateGraphicsDevice() => null;
        static void PrintLine(string output) => Console.WriteLine($"[{DateTime.Now}] {output}");

        public ConsoleDemo()
        {
            PrintLine($"IsInputRedirected: {Console.IsInputRedirected}");
            PrintLine($"LimitFrameRate: {LimitFrameRate = false}");
            PrintLine($"TargetUpdatesPerSecond: {TargetUpdatesPerSecond = 60.0}");
        }

        protected override void CreateResources()
        {
            PrintLine("Creating resources");
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
            ++frameCount;
            if (renderStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats("Render", dt, frameCount, updateCount, startTime);
                renderStopwatch.Restart();
            }
        }

        protected override void Update(double dt)
        {
            ++updateCount;
            if (updateStopwatch.Elapsed.TotalSeconds > 2)
            {
                printStats("Update", dt, frameCount, updateCount, startTime);
                updateStopwatch.Restart();
            }
        }

        private void printStats(string label, double dt, uint frameCount, uint updateCount, DateTime startTime)
        {
            var fps = frameCount / (DateTime.Now - startTime).TotalSeconds;
            var ups = updateCount / (DateTime.Now - startTime).TotalSeconds;
            PrintLine($"[{label}] Dt: {dt:0.####} Frames: {frameCount} @ {fps:N} hz Updates: {updateCount} @ {ups:N} hz");
        }
    }
}