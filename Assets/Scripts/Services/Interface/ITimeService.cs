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
    }
}
