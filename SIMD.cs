using System;
using System.Numerics;
using System.Runtime.CompilerServices;

class SIMD
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        for(int i=0; i<result.Length; i+=Vector<int>.Count)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va + vb).CopyTo(result, i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        for(int i=0; i<result.Length; i+=Vector<int>.Count)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va * vb).CopyTo(result, i);
        }
    }
}
