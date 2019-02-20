﻿using System.Numerics;
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
    struct VertexPositionColor
    {
        public Vector2 Position;
        public RgbaFloat Color;
        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
        public const uint SizeInBytes = 24;
    }

    public sealed class Demo : Engine.Application
    {
        static void Main() => Demo.Instance.Run();
        static Demo Instance => _instance;
        static readonly Demo _instance = new Demo();

        uint width = 800, height = 800, viewScale = 1;
        Sdl2Window window;
        CommandList commandList;
        DeviceBuffer vertexBuffer, indexBuffer;
        Shader[] shaders;
        Pipeline pipeline;
        Stopwatch stopwatch = new Stopwatch();

        public Demo(bool LimitFrameRate = false) 
            : base(LimitFrameRate)
        {
        }

        protected override GraphicsDevice CreateGraphicsDevice()
        {
            GraphicsDevice graphicsDevice;
            GraphicsBackend backend = VeldridStartup.GetPlatformDefaultBackend();

            VeldridStartup.CreateWindowAndGraphicsDevice(
                 new WindowCreateInfo(100, 100, (int)(width * viewScale), (int)(height * viewScale), WindowState.Normal, ""),
                 new GraphicsDeviceOptions(debug: false, swapchainDepthFormat: null, syncToVerticalBlank: false),
                 GraphicsBackend.OpenGLES,
                //  backend,
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

        private Shader[] createShaders()
        {
            ShaderDescription vertexShaderDesc = new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(File.ReadAllText("./Shaders/VertexShader.hlsl")), "main");
            ShaderDescription fragmentShaderDesc = new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(File.ReadAllText("./Shaders/FragmentShader.hlsl")), "main");
            return GraphicsDevice.ResourceFactory.CreateFromSpirv(vertexShaderDesc, fragmentShaderDesc);
        }

        private VertexLayoutDescription createVertexLayout()
        {
            var vertexElementDescriptionPosition = new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2);
            var vertexElementDescriptionColor = new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4);
            return new VertexLayoutDescription(vertexElementDescriptionPosition, vertexElementDescriptionColor);
        }

        private Pipeline createPipeline(VertexLayoutDescription vertexLayout)
        {
            GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription();
            pipelineDescription.BlendState = BlendStateDescription.SingleOverrideBlend;

            pipelineDescription.DepthStencilState = new DepthStencilStateDescription(
                depthTestEnabled: true,
                depthWriteEnabled: true,
                comparisonKind: ComparisonKind.LessEqual);

            pipelineDescription.RasterizerState = new RasterizerStateDescription(
                cullMode: FaceCullMode.Back,
                fillMode: PolygonFillMode.Solid,
                frontFace: FrontFace.Clockwise,
                depthClipEnabled: true,
                scissorTestEnabled: false);

            pipelineDescription.PrimitiveTopology = PrimitiveTopology.TriangleStrip;
            pipelineDescription.ResourceLayouts = System.Array.Empty<ResourceLayout>();
            pipelineDescription.ShaderSet = new ShaderSetDescription(vertexLayouts: new VertexLayoutDescription[] { vertexLayout }, shaders: shaders);
            pipelineDescription.Outputs = GraphicsDevice.SwapchainFramebuffer.OutputDescription;
            return GraphicsDevice.ResourceFactory.CreateGraphicsPipeline(pipelineDescription);
        }

        protected override void CreateResources()
        {
            var factory = GraphicsDevice.ResourceFactory;
            VertexPositionColor[] quadVertices = {
                new VertexPositionColor(new Vector2(-1.0f, 1.0f), RgbaFloat.Red),
                new VertexPositionColor(new Vector2(1.0f, 1.0f), RgbaFloat.Green),
                new VertexPositionColor(new Vector2(-1.0f, -1.0f), RgbaFloat.Blue),
                new VertexPositionColor(new Vector2(1.0f, -1.0f), RgbaFloat.Yellow)
            };
            ushort[] quadIndices = { 0, 1, 2, 3 };

            vertexBuffer = factory.CreateBuffer(new BufferDescription(4 * VertexPositionColor.SizeInBytes, BufferUsage.VertexBuffer));
            indexBuffer = factory.CreateBuffer(new BufferDescription(4 * sizeof(ushort), BufferUsage.IndexBuffer));
            GraphicsDevice.UpdateBuffer(vertexBuffer, 0, quadVertices);
            GraphicsDevice.UpdateBuffer(indexBuffer, 0, quadIndices);

            shaders = createShaders();
            var vertexLayout = createVertexLayout();
            pipeline = createPipeline(vertexLayout);
            commandList = factory.CreateCommandList();
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        protected override void Render(double dt)
        {
            var cl = commandList as CommandList;
            cl.Begin();
            cl.SetFramebuffer(GraphicsDevice.MainSwapchain.Framebuffer);
            cl.ClearColorTarget(0, RgbaFloat.Black);
            cl.SetVertexBuffer(0, vertexBuffer);
            cl.SetIndexBuffer(indexBuffer, IndexFormat.UInt16);
            cl.SetPipeline(pipeline);
            cl.DrawIndexed(
                indexCount: 4,
                instanceCount: 1,
                indexStart: 0,
                vertexOffset: 0,
                instanceStart: 0
            );
            cl.End();
            GraphicsDevice.SubmitCommands(cl);
            base.Render(dt);
        }

        protected override void Update(double dt)
        {
            if (!window.Exists)
            {
                Exit();
                return;
            }
            var input = window.PumpEvents();
            if (input.KeyEvents.Count > 0)
            {
                if (input.KeyCharPresses.Contains('l'))
                {
                    LimitFrameRate = !LimitFrameRate;
                    Console.WriteLine($"LimitFramerate: {LimitFrameRate}");
                }
                if(input.KeyCharPresses.Contains('q'))
                {
                    Exit();
                }
            }
            
            if(stopwatch.Elapsed.Milliseconds > 500)
            {   
                var frameTime = gameTime.ElapsedGameTime.Milliseconds;
                window.Title = $"FrameTime: {1000/FramesPerSecond:0.##} ms, Fps: {FramesPerSecond}, TotalFrames: {TotalFrames}, Width: {width}, Height: {height}";
                stopwatch.Restart();
            }
        }

        public override void Dispose()
        {
            commandList?.Dispose();
            pipeline?.Dispose();
            vertexBuffer?.Dispose();
            indexBuffer?.Dispose();
            base.Dispose();
        }
    }
}
