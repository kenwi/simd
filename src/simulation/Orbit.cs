using System;
using System.Numerics;

namespace simd
{
    public class Orbit
    {
        public static Vector2 Calculate(float angle, float a, float b, float theta, Vector2 p, float numPertubations, float pertubationAmplitude)
        {
            double beta = -angle,
                   alpha = theta * 2 * Math.PI;

            double cosalpha = Math.Cos(alpha),
                    sinalpha = Math.Sin(alpha),
                    cosbeta = Math.Cos(beta),
                    sinbeta = Math.Sin(beta);

            var pos = new Vector2(p.X + (float)(a * cosalpha * cosbeta - b * sinalpha * sinbeta), 
                                    p.Y + (float)(a * cosalpha * sinbeta + b * sinalpha * cosbeta));

            if (pertubationAmplitude > 0 && numPertubations > 0)
            {
                pos.X += (a / pertubationAmplitude) * (float)Math.Sin(alpha * 2 * numPertubations);
                pos.Y += (a / pertubationAmplitude) * (float)Math.Cos(alpha * 2 * numPertubations);
            }
            return pos;
        }
    }
}