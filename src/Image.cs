using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using Image = SixLabors.ImageSharp.Image;
using System;

namespace simd
{
    class ImageWriter
    {
        public static void Write(ref int[] buffer, string file, int width, int height)
        {
            var image = new System.Drawing.Bitmap(width, height);
            for (int i = 0; i < buffer.Length - 1; i++)
            {
                var index = ArrayIndex.From1DTo2D(i, width);
                var color = Color.FromArgb(buffer[i], buffer[i], buffer[i]);
                image.SetPixel((int)index.X, (int)index.Y, color);
            }
            image.Save(file);
        }

        public static void FastWriteref(ref Rgba32[] buffer, string file, int width, int height)
        {
            using (var image = Image.LoadPixelData<Rgba32>(buffer, width, height))
            {
                image.Save(file);
            }
        }

        public static void FastWrite(Span<Rgba32> buffer, string file, int width, int height)
        {
            using (var image = Image.LoadPixelData<Rgba32>(buffer, width, height))
            {
                image.Save(file);
            }
        }

        public static void FastWrite(ref Image<Rgba32> buffer, string file, int width, int height)
        {
            buffer.Save(file);
        }
    }
}
