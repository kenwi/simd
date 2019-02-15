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

            return graphicsDevice;
        }
    }
}
