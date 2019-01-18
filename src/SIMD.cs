using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

class SIMD
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ExecuteOnSets(ref int[] a, ref int[] b, ref int[] result, Func<Vector<int>, Vector<int>, Vector<int>> method)
    {          
        for(int i=0; i<result.Length; i+=Vector<int>.Count)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            method(va, vb).CopyTo(result, i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref int[] a, ref int[] b, ref int[] result)
    {
        for(int i=0; i<result.Length; i+=Vector<int>.Count)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va + vb).CopyTo(result, i);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ref int[] a, ref int[] b, ref int[] result)
    {
        for(int i=0; i<result.Length; i+=Vector<int>.Count)
        {
            var va = new Vector<int>(a, i);
            var vb = new Vector<int>(b, i);
            (va * vb).CopyTo(result, i);
        }
    }
}
