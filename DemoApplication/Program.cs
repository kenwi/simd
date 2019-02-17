using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace DemoApplication
{
    public sealed class Demo : Engine.Application
    {
        static void Main() => Demo.Instance.Run();
        static Demo Instance => _instance;
        static readonly Demo _instance = new Demo();

        uint width = 1024, height = 768, viewScale = 1;
        Sdl2Window window;
        CommandList commandList;

        uint totalFrames = 0;

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            GraphicsDevice graphicsDevice;
            GraphicsBackend backend = VeldridStartup.GetPlatformDefaultBackend();
            
            VeldridStartup.CreateWindowAndGraphicsDevice(
                 new WindowCreateInfo(100, 100, (int)(width * viewScale), (int)(height * viewScale), WindowState.Normal, ""),
                 new GraphicsDeviceOptions(debug: false, swapchainDepthFormat: null, syncToVerticalBlank: false),
                 backend,
                 out window,
                 out graphicsDevice);

            window.CursorVisible = true;
            window.Closing += Exit;
            window.Closed += Dispose;

            return graphicsDevice;
        }

        protected override void Render(float dt)
        {
            base.Render(dt);
        }

        protected override void Update(float dt)
        {
            if(!window.Exists)
            {
                Exit();
                return;
            }
            var input = window.PumpEvents();
            window.Title = $"Numframes: {++totalFrames}";
            base.Update(dt);
        }

        public override void Dispose()
        {
            commandList?.Dispose();
            base.Dispose();
        }
    }
}
