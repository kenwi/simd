using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    internal class FrameBuffer
    {
        private int width;
        private int height;
        private Image<Rgba32> frame;

        public FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            frame = new Image<Rgba32>(width, height);
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
    }
}