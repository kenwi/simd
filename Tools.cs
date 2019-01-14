using System.Runtime.CompilerServices;

namespace heightmap_simd
{
        static class ArrayIndex
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int From2DTo1D(int x, int y, int width)
        {
            return (y * width) + x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int[] From1DTo2D(int index, int width)
        {
            return new int[] {
                index / width,
                index % width
            };
        }
    }
}
