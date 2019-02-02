using System;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    internal class FrameBuffer
    {
        private int width;
        private int height;
        private Rgba32[] frame;

        public FrameBuffer(int width, int height)
        {
            this.width = width;
            this.height = height;
            frame = new Rgba32[width * height];
        }

        internal void MakeTestBuffer()
        {
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    frame[i + j * width] = new Rgba32(j / (float)height, i / (float)width, 0.0f);
                }
            }
        }

        internal void Write(string fileName)
        {
            ImageWriter.FastWrite(ref frame, fileName, width, height);
        }
    }
}