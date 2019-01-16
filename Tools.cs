using System.Numerics;
using System.Runtime.CompilerServices;

namespace simd
{
    public static class ArrayIndex
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int From2DTo1D(int x, int y, int width)
        {
            return (x + width) * y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2  From1DTo2D(int index, int width)
        {
            return new Vector2(index % width, index / width);
        }
    }
}
