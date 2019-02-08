using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace simd
{
    public static class Noise
    {
        // http://www.iquilezles.org/www/articles/morenoise/morenoise.htm
        public static Vector4 noised(Vector3 x)
        {
            Func<Vector3, float, Vector3> multiply = (v, n) => new Vector3(v.X * n, v.Y * n, v.Z * n);
            Func<Vector3, float, Vector3> minus = (v, n) => new Vector3(v.X - n, v.Y - n, v.Z - n);
            Func<Vector3, float, Vector3> add = (v, n) => new Vector3(v.X + n, v.Y + n, v.Z + n);
            Func<Vector3, Vector3> floor = v => new Vector3((float)Math.Floor(v.X), (float)Math.Floor(v.Y), (float)Math.Floor(v.Z));
            Func<Vector3, Vector3> fract = v => v - floor(v);

            var fn = new FastNoise();
            Func<Vector3, float> myRandomMagic = (n) => fn.GetValue(n.X, n.Y, n.Z);

            Vector3 p = floor(x);
            Vector3 w = fract(x);

            Vector3 u = w * w * w * (w * (new Vector3(w.X * 6.0f - 15.0f, w.Y * 6.0f - 15.0f, w.Z * 6.0f - 15.0f)));
            Vector3 du = multiply(w, 30) * w * add(w * minus(w, 2.0f), 1.0f);

            float a = myRandomMagic(p + new Vector3(0, 0, 0));
            float b = myRandomMagic(p + new Vector3(1, 0, 0));
            float c = myRandomMagic(p + new Vector3(0, 1, 0));
            float d = myRandomMagic(p + new Vector3(1, 1, 0));
            float e = myRandomMagic(p + new Vector3(0, 0, 1));
            float f = myRandomMagic(p + new Vector3(1, 0, 1));
            float g = myRandomMagic(p + new Vector3(0, 1, 1));
            float h = myRandomMagic(p + new Vector3(1, 1, 1));

            float k0 = a;
            float k1 = b - a;
            float k2 = c - a;
            float k3 = e - a;
            float k4 = a - b - c + d;
            float k5 = a - c - e + g;
            float k6 = a - b - e + f;
            float k7 = -a + b + c - d + e - f - g + h;

            Vector3 derivatives = multiply(du, 2.0f) * new Vector3(k1 + k4 * u.Y + k6 * u.Z + k7 * u.Y * u.Z, k2 + k5 * u.Z + k4 * u.X + k7 * u.Z * u.X, k3 + k6 * u.X + k5 * u.Z + k7 * u.X * u.Y);
            float noiseValue = -1.0f + 2.0f * (k0 + k1 * u.X + k2 * u.Y + k3 * u.Z + k4 * u.X * u.Y + k5 * u.Y * u.Z + k6 * u.Z * u.X + k7 * u.X * u.Y * u.Z);
            return new Vector4(derivatives, noiseValue);
        }
    }

    public static class ArrayIndex
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int From2DTo1D(int x, int y, int width)
        {
            return (width * y) + x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 From1DTo2D(int index, int width)
        {
            return new Vector2(index % width, index / width);
        }
    }
}
