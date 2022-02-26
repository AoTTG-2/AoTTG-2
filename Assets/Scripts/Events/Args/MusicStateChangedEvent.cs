using Assets.Scripts.Audio;

namespace Assets.Scripts.Events.Args
{
    public class MusicStateChangedEvent
    {
        #region Constructors
        public MusicStateChangedEvent(MusicState state, int keepStateActive = default)
        {
            State = state;
            KeepStateActive = keepStateActive;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the active <see cref="MusicState"/>.
        /// </summary>
        public MusicState State { get; }

        /// <summary>
        /// Gets the time in seconds current state is set to be active <see cref="int"/>.
        /// </summary>
        public int KeepStateActive { get; }
        #endregion
    }
}
