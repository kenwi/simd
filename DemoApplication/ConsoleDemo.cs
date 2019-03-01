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
        BackgroundWorker inputBackgroundWorker = new BackgroundWorker();
        uint numFrames = 0, numUpdates = 0;

        static void PrintLine(string output) => Console.WriteLine($"[{DateTime.Now}] {output}");

        public ConsoleDemo()
        {
            this.LimitFrameRate = false;
            this.inputBackgroundWorker.DoWork += (s, e) => GetEvents();
            this.inputBackgroundWorker.RunWorkerAsync();
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
            if(renderStopwatch.Elapsed.TotalSeconds > 2)
            {
                PrintLine($"[Render] Dt: {dt:0.####}, Frames: {numFrames}, Updates: {numUpdates}");
                renderStopwatch.Restart();
            }
        }

        protected override void Update(double dt)
        {
            numUpdates++;
            if(updateStopwatch.Elapsed.TotalSeconds > 2)
            {
                PrintLine($"[Update] Dt: {dt:0.####}, Frames: {numFrames}, Updates: {numUpdates}");
                updateStopwatch.Restart();
            }
        }
    }
}