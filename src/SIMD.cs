using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using SixLabors.ImageSharp.PixelFormats;

namespace simd
{
    class SIMD
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ExecuteOnSets(ref int[] a, ref int[] b, ref int[] result, Func<Vector<int>, Vector<int>, Vector<int>> method)
        {
            int i = 0, vecSize = Vector<int>.Count;
            for (; i < result.Length - vecSize; i += vecSize)
            {
                var va = new Vector<int>(a, i);
                var vb = new Vector<int>(b, i);
                method(va, vb).CopyTo(result, i);
            }
            if (i != result.Length)
            {
                for (; i < result.Length; i++)
                {
                    var va = new Vector<int>(a, i);
                    var vb = new Vector<int>(b, i);
                    method(va, vb).CopyTo(result, i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(ref int[] a, ref int[] b, ref int[] result)
        {
            int i = 0, vecSize = Vector<int>.Count;
            for (; i < result.Length - vecSize; i += vecSize)
            {
                var va = new Vector<int>(a, i);
                var vb = new Vector<int>(b, i);
                (va + vb).CopyTo(result, i);
            }
            if (i != result.Length)
            {
                for (; i < result.Length; i++)
                {
                    result[i] = a[i] + b[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply(ref int[] a, ref int[] b, ref int[] result)
        {
            int i = 0, vecSize = Vector<int>.Count;
            for (; i < result.Length - vecSize; i += vecSize)
            {
                var va = new Vector<int>(a, i);
                var vb = new Vector<int>(b, i);
                (va * vb).CopyTo(result, i);
            }
            if (i != result.Length)
            {
                for (; i < result.Length; i++)
                {
                    result[i] = a[i] * b[i];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DotProduct(ref Vector3[] a, ref Vector3[] b, ref float[] result)
        {
            for(int i=0; i<result.Length; i++)
            {
                result[i] = Vector3.Dot(a[i], b[i]);
            }
            /*int i, vecSize = Vector<Vector3>.Count;
            for (i = 0; i <  result.Length - vecSize; i += vecSize)
            {
                var va = new Vector<Vector3>(a, i);
                var vb = new Vector<Vector3>(b, i);
                Vector.Dot(va, vb).CopyTo(result, i);       
            }
            if (i != result.Length)
            {
                for (; i < vecSize; ++i)
                {
                    result[i] = Vector3.Dot(a[i], b[i]);
                }
            }*/
        }
    }
}