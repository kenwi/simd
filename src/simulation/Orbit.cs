using System;
using System.Numerics;

namespace simd
{
    public class Orbit
    {
        static double PC_TO_KM = 3.08567758129e13;
        static double SEC_PER_YEAR = 365.25 * 86400;

        public static void Calculate(float angle, float a, float b, float theta, ref float px, ref float py, float numPertubations, float pertubationAmplitude)
        {
            double beta = -angle,
                   alpha = theta * 2 * Math.PI;

            double cosalpha = Math.Cos(alpha),
                    sinalpha = Math.Sin(alpha),
                    cosbeta = Math.Cos(beta),
                    sinbeta = Math.Sin(beta);

            px = px + (float)(a * cosalpha * cosbeta - b * sinalpha * sinbeta);
            py = py + (float)(a * cosalpha * sinbeta + b * sinalpha * cosbeta);

            if (pertubationAmplitude > 0 && numPertubations > 0)
            {
                px += (a / pertubationAmplitude) * (float)Math.Sin(alpha * 2 * numPertubations);
                px += (a / pertubationAmplitude) * (float)Math.Cos(alpha * 2 * numPertubations);
            }
        }

        public double CalculateOrbitalVelocity(float radius)
        {
            Func<double, double> MS = (r) =>
            {
                double d = 2000;
                double rho_so = 1;
                double rH = 2000;
                return rho_so * Math.Exp(-r / rH) * (r * r) * Math.PI * d;
            };

            Func<double, double> MH = (r) =>
            {
                double rho_h0 = 0.15;
                double rC = 2500;
                return rho_h0 * 1 / (1 + Math.Pow(r / rC, 2)) * (4 * Math.PI * Math.Pow(r, 3) / 3);
            };

            Func<double, double> vd = (r) =>
            {
                double MZ = 1000;
                double G = 6.672e-11;
                return 20000 * Math.Sqrt(G * (MS(r) + MZ) / r);
            };

            double vel_kms = vd(radius);
            double u = 2 * Math.PI * radius * PC_TO_KM;
            double time = u / (vel_kms * SEC_PER_YEAR);

            return 360.0 / time;
        }
    }
}