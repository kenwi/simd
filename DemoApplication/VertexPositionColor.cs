using System.Numerics;
using Veldrid;

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
}