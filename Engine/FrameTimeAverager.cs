namespace Engine
{
    public class FrameTimeAverager
    {
        private readonly double decayRate = .3;
        private readonly double timeLimit;

        private double accumulatedTime;
        private int frameCount;
        private int totalFrames;

        public FrameTimeAverager(double maxTimeSeconds = 666)
        {
            timeLimit = maxTimeSeconds;
        }

        public double CurrentAverageFrameTimeSeconds { get; private set; }
        public double CurrentAverageFrameTimeMilliseconds => CurrentAverageFrameTimeSeconds * 1000.0;
        public double CurrentAverageFramesPerSecond => 1 / CurrentAverageFrameTimeSeconds;
        public int TotalFrames => totalFrames;

        public void Reset()
        {
            accumulatedTime = 0;
            frameCount = 0;
        }

        public void AddTime(double seconds)
        {
            accumulatedTime += seconds;
            frameCount++;
            totalFrames++;
            if (accumulatedTime >= timeLimit) 
                Average();
        }

        private void Average()
        {
            var total = accumulatedTime;
            CurrentAverageFrameTimeSeconds =
                CurrentAverageFrameTimeSeconds * decayRate
                + total / frameCount * (1 - decayRate);

            accumulatedTime = 0;
            frameCount = 0;
        }
    }
}
