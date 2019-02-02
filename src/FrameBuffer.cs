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
        }

        internal void MakeTestBuffer()
        {
            frame = new Image<Rgba32>(width, height);
            for (int y = 0; y < frame.Height; y++)
            {
                var row = frame.GetPixelRowSpan(y);
                for (int x = 0; x < frame.Width; x++)
                {
                    row[x] = new Rgba32(y / (float)height, x / (float)width, 0.0f);
                }
            }
        }

        internal void Write(string fileName)
        {
            ImageWriter.FastWrite(ref frame, fileName, width, height);
            frame.Dispose();
        }
    }
}