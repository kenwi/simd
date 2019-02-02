namespace simd
{
    public class Particles
    {
        public float[] theta;
        public float[] vTheta;
        public float[] angle;
        public float[] centerX;
        public float[] centerY;
        public float[] velocityX;
        public float[] velocityY;
        public float[] positionX;
        public float[] positionY;
        public float[] a;
        public float[] b;

        public int numParticles;

        public Particles(int numParticles)
        {
            theta = new float[numParticles];
            vTheta = new float[numParticles];
            angle = new float[numParticles];
            centerX = new float[numParticles];
            centerY = new float[numParticles];
            velocityX = new float[numParticles];
            velocityY = new float[numParticles];
            positionX = new float[numParticles];
            positionY = new float[numParticles];
            a = new float[numParticles];
            b = new float[numParticles];
            this.numParticles = numParticles;
        }
    }
}