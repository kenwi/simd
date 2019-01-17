using System;
using System.Numerics;

namespace simd
{
    class SphericalGeometry
    {
        public Vector3[] GetRandomSurfaceField(int[] grid, Random random)
        {
            var zMultipliers = new Vector3[1000];
            for(int i=0; i<1000; i++)
            {
                zMultipliers[i] = GetRandomPointOnSurface(random);
            }

            var z = new Raster(grid);
            for(int i=0; i<zMultipliers.Length; i++)
            {

            }

            throw new NotImplementedException("Finish your shait!");
            return zMultipliers;
        }

        private Vector3 GetRandomPointOnSurface(Random random)
        {
            return SphericalToCartesian(
                Math.Asin(2 * random.NextDouble() - 1),
                2 * Math.PI * random.NextDouble()
            );
        }

        private Vector3 SphericalToCartesian(double latitude, double longitude)
        {
            return new Vector3(
                (float)(Math.Cos(latitude) * Math.Cos(longitude)),
                (float)Math.Sin(latitude),
                (float)(-Math.Cos(latitude) * Math.Sin(longitude))
            );
        }
    }
}