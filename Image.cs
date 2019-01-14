using System.IO;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

class ImageWriter 
{
    public static void Write(ref int[] buffer, string file, int width, int height)
    {
        using(var output = File.OpenWrite(file))
        {
            Image<Rgba32> image = new Image<Rgba32>(width, height);
            image.Save(file);
        }
    }
}