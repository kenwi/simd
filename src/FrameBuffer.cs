using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    internal class FrameBuffer
    {
        private Image<Rgba32> frame;
        
        public int width;
        public int height;
        public Vector2 center;

        public FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.center = new Vector2(width / 2, height / 2);
            this.frame = new Image<Rgba32>(width, height);
        }

        public Span<Rgba32> GetFrameBuffer()
        {
            return frame.GetPixelSpan();
        }

        internal void MakeTestBuffer()
        {
            for (int y = 0; y < frame.Height; y++)
            {
                var row = frame.GetPixelRowSpan(y);
                for (int x = 0; x < frame.Width; x++)
                {
                    row[x] = new Rgba32(y / (float)height, x / (float)width, 0.0f);
                }
            }
        }

        internal void SetPixels(Func<int, int, Rgba32> function)
        {
            for (int y = 0; y < frame.Height; y++)
            {
                var row = frame.GetPixelRowSpan(y);
                for (int x = 0; x < frame.Width; x++)
                {
                    row[x] = function(x, y);
                }
            }
        }

        internal void Write(string fileName)
        {
            ImageWriter.FastWrite(ref frame, fileName, width, height);
            frame = new Image<Rgba32>(width, height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float wrapAround(float coordinate, float max)
        {
            coordinate %= max + 1;
            if (coordinate < 0)
                coordinate += max;
            return coordinate;
        }
        
        internal void Update(ref float[] positionX, ref float[] positionY)
        {
            frame = new Image<Rgba32>(width, height);
            var pixels = frame.GetPixelSpan();
            for (int i = 0; i < positionX.Length; i++)
            {
                var px  = wrapAround(positionX[i], width - 1);
                var py = wrapAround(positionY[i], height - 1);

                var index = ArrayIndex.From2DTo1D((int)px, (int)py, width);
                pixels[index] = new Rgba32(255, 255, 255);
            }
        }
    }
}