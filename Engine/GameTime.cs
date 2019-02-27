using System;

namespace Engine
{
    public class GameTime
    {
        public GameTime()
        {
            TotalGameTime = TimeSpan.Zero;
            ElapsedGameTime = TimeSpan.Zero;
            IsRunningSlowly = false;
        }

        public GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = false;
        }

        private GameTime(TimeSpan totalGameTime, TimeSpan elapsedGameTime, bool isRunningSlowly)
        {
            TotalGameTime = totalGameTime;
            ElapsedGameTime = elapsedGameTime;
            IsRunningSlowly = isRunningSlowly;
        }

        /// <summary>
        /// The amount of game time since the start of the game.
        /// </summary>
        /// <value>
        /// Game time since the start of the game.
        /// </value>
        public TimeSpan TotalGameTime { get; }

        /// <summary>
        /// The amount of elapsed game time since the last update.
        /// </summary>
        public TimeSpan ElapsedGameTime { get; }

        /// <summary>
        /// Gets a value indicating that the game loop is taking longer than its TargetElapsedTime. In this case, the
        /// game loop can be considered to be running too slowly and should do something to "catch up."
        /// </summary>
        /// <value>
        /// true if the game loop is taking too long; false otherwise.
        /// </value>
        public bool IsRunningSlowly { get; }

        public static GameTime RunningSlowly(GameTime gameTime)
        {
            return new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime, true);
        }
    }
}
