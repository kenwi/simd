using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace heightmap_simd
{
    class ImageWriter
    {
        public static void Write(ref int[] buffer, string file, int width, int height)
        {
            var image = new System.Drawing.Bitmap(width, height);
            for (int i = 0; i < buffer.Length - 1; i++)
            {
                var index = ArrayIndex.From1DTo2D(i, width);
                var color =   Color.FromArgb(buffer[i], buffer[i], buffer[i]);
                image.SetPixel(index[0], index[1], color);
                
            }
            image.Save(file);
        }

        public static void FastWrite(string file, ref byte[] buffer, int width, int height)
        {
            var ms = new MemoryStream(buffer);
            var image = Image.FromStream(ms);
        }
    }
}
