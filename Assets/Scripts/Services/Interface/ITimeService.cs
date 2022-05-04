namespace Assets.Scripts.Services.Interface
{
    public interface ITimeService
    {

        /// <summary>
        /// Returns the time since the room was created. Timestamp is set OnCreatedRoom
        /// </summary>
        float GetRoomTime();

        /// <summary>
        /// Returns the time since the round has started. Timestamp is set OnLevelWasLoaded
        /// </summary>
        float GetRoundTime();

        /// <summary>
        /// Returns the round time as a rounded integer
        /// </summary>
        int GetRoundDisplayTime();

        /// <summary>
        /// Returns the time since the round started while the game has been unpaused.
        /// </summary>
        /// <returns></returns>
        float GetUnPausedRoundTime();

        /// <summary>
        /// Returns the time since the round started while the game has been unpaused rounded as an integer.
        /// </summary>
        /// <returns></returns>
        int GetUnPausedRoundDisplayTime();

        /// <summary>
        /// Returns the total time the game has been paused for since the start of the round.
        /// </summary>
        /// <returns></returns>
        float GetTotalPausedTime();
    }
}
