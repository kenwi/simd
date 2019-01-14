using System;
using System.Numerics;
using System.Runtime.CompilerServices;

class SIMD
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        var simdLength = Vector<int>.Count;
        int i;
        for (i = 0; i <= a.Length - simdLength; i += simdLength)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va + vb).CopyTo(result, i);
        }
        for (; i < a.Length; ++i)
        {
            result[i] = a[i] + b[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        var simdLength = Vector<int>.Count;
        int i;
        for (i = 0; i <= a.Length - simdLength; i += simdLength)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va * vb).CopyTo(result, i);
        }
        for (; i < a.Length; ++i)
        {
            result[i] = a[i] * b[i];
        }
    }
}
