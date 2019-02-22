using System.Numerics;
using System.Text;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Veldrid.SPIRV;
using System;
using System.Linq;
using System.Diagnostics;
using File = System.IO.File;

namespace DemoApplication
{
    public abstract class Demo : Engine.Application
    {
        protected uint width = 800, height = 800, viewScale = 1;
        protected Sdl2Window window;

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            GraphicsDevice graphicsDevice;
            GraphicsBackend backend = VeldridStartup.GetPlatformDefaultBackend();

            VeldridStartup.CreateWindowAndGraphicsDevice(
                 new WindowCreateInfo(100, 100, (int)(width * viewScale), (int)(height * viewScale), WindowState.Normal, ""),
                 new GraphicsDeviceOptions(debug: false, swapchainDepthFormat: null, syncToVerticalBlank: false),
                 GraphicsBackend.OpenGLES,
                 out window,
                 out graphicsDevice);
            window.CursorVisible = true;
            window.Closing += Exit;
            window.Closed += Dispose;
            window.Resized += () =>
            {
                width = (uint)window.Width * viewScale;
                height = (uint)window.Height * viewScale;
            };
            return graphicsDevice;
        }
    }
}
