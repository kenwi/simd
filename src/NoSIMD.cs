using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace simd
{
    class NoSIMD
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref int[] a, ref int[] b, ref int[] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] + b[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref int[] a, ref int[] b, ref int[] result)
        {
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] * b[i];
            }
        }
    }

}