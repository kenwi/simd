using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;
using File = System.IO.File;

namespace DemoApplication
{
    public sealed class RayMarchingDemo : Demo
    {
        public static RayMarchingDemo Instance => _instance;
        static readonly RayMarchingDemo _instance = new RayMarchingDemo();

        DeviceBuffer vertexBuffer, indexBuffer;
        CommandList commandList;
        Shader[] shaders;
        Pipeline pipeline;
        Stopwatch stopwatch = new Stopwatch();
        InputSnapshot inputSnapshot;

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
            pipeline = createPipeline(createVertexLayout());
            commandList = factory.CreateCommandList();
            stopwatch.Start();
        }

        private Shader[] createShaders()
        {
            var vertexShaderDesc = new ShaderDescription(ShaderStages.Vertex, Encoding.UTF8.GetBytes(File.ReadAllText("./Shaders/RayMarchingDemo/VertexShader.hlsl")), "main");
            var fragmentShaderDesc = new ShaderDescription(ShaderStages.Fragment, Encoding.UTF8.GetBytes(File.ReadAllText("./Shaders/RayMarchingDemo/FragmentShader.hlsl")), "main");
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
            pipelineDescription.ShaderSet = new ShaderSetDescription(
                vertexLayouts: new VertexLayoutDescription[] { vertexLayout },
                shaders: shaders);
            pipelineDescription.Outputs = GraphicsDevice.SwapchainFramebuffer.OutputDescription;

            return GraphicsDevice.ResourceFactory.CreateGraphicsPipeline(pipelineDescription);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Render()
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
            base.Render();
        }

        protected override void GetUserInput()
        {
            inputSnapshot = window.PumpEvents();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Update(double dt)
        {
            if (!window.Exists)
            {
                Exit();
                return;
            }
            if (inputSnapshot.KeyCharPresses.Count > 0)
            {
                if (inputSnapshot.KeyCharPresses.Contains('l'))
                {
                    Console.WriteLine($"LimitFramerate: {LimitFrameRate = !LimitFrameRate}");
                }
                if (inputSnapshot.KeyCharPresses.Contains('q'))
                {
                    Exit();
                }
            }

            if (stopwatch.Elapsed.Milliseconds > 500)
            {
                window.Title = $"FrameTime: {FramesPerSecond * 1e-4:0.##} ms, Fps: {FramesPerSecond}, TotalFrames: {TotalFrames}, Width: {width}, Height: {height}";
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