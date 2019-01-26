using System.Numerics;
using System.Runtime.CompilerServices;

namespace simd
{
    public class CartesianSimulationComputation
    {
        int numParticles;
        public Vector<float> px, py;
        public Vector<float> ax, ay;
        public Vector<float> vx, vy;

        public CartesianSimulationComputation(int numParticles)
        {
            this.numParticles = numParticles;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Simulate(float dt, ref float[] PX, ref float[] PY, ref float[] VX, ref float[] VY, ref float[] AX, ref float[] AY)
        {
            int i, vecSize = Vector<float>.Count;
            for(i=0; i <= numParticles - vecSize; i += vecSize)
            {
                ax = new Vector<float>(AX, i);
                ay = new Vector<float>(AY, i);
                vx = new Vector<float>(VX, i);
                vy = new Vector<float>(VY, i);
                px = new Vector<float>(PX, i);
                py = new Vector<float>(PY, i);

                vx = vx + (ax * dt);
                vy = vy + (ay * dt);
                px = px + vx;
                py = py + vy;
                ax.CopyTo(AX, i);
                ay.CopyTo(AY, i);
                vx.CopyTo(VX, i);
                vy.CopyTo(VY, i);
                px.CopyTo(PX, i);
                py.CopyTo(PY, i);
            }
            for(; i<numParticles; i++)
            {
                AX[i] = ax[i] * dt;
                AY[i] = ay[i] * dt;
                VX[i] = vx[i] + ax[i] * dt;
                VY[i] = vy[i] + ay[i] * dt;
                PX[i] = px[i] + vx[i];
                PY[i] = py[i] + vy[i];
            }
        } 
    }
}