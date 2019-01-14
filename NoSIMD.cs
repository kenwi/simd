using System;
using System.Numerics;
using System.Runtime.CompilerServices;

class NoSIMD
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Add(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] + b[i];
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Multiply(ref int[] a, ref int[] b, out int[] result)
    {
        result = new int[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            result[i] = a[i] * b[i];
        }
    }
}
