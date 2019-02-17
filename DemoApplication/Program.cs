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

        uint width = 1024, height = 768, viewScale = 1, totalFrames = 0;
        Sdl2Window window;
        CommandList commandList;

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
            commandList = graphicsDevice.ResourceFactory.CreateCommandList();
            window.CursorVisible = true;
            window.Closing += Exit;
            window.Closed += Dispose;
            window.Resized += () => {
                width = (uint)window.Width * viewScale;
                height = (uint)window.Height * viewScale;
            };
            return graphicsDevice;
        }

        protected override void Render(float dt)
        {
            var cl = commandList as CommandList;
            cl.Begin();
            cl.SetFramebuffer(GraphicsDevice.MainSwapchain.Framebuffer);
            //cl.Draw(3);
            cl.End();
            GraphicsDevice.SubmitCommands(cl);
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
            window.Title = $"Numframes: {++totalFrames}, Width: {width}, Height: {height}";
            base.Update(dt);
        }

        public override void Dispose()
        {
            commandList?.Dispose();
            base.Dispose();
        }
    }
}
