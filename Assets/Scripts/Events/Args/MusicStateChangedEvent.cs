using Assets.Scripts.Audio;
using System;

namespace Assets.Scripts.Events.Args
{
    public class MusicStateChangedEvent : EventArgs
    {
        #region Constructors
        public MusicStateChangedEvent(MusicState activeState)
        {
            ActiveState = activeState;
        }

        public MusicStateChangedEvent(MusicState activeState, int keepStateActive)
        {
            ActiveState = activeState;
            KeepStateActive = keepStateActive;
        }

        public MusicStateChangedEvent(MusicState? previousState, MusicState activeState, int keepStateActive, bool playFromBegining)
        {
            ActiveState = activeState;
            PreviousActiveState = previousState;
            KeepStateActive = keepStateActive;
            PlayFromBegining = playFromBegining;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the previously active <see cref="MusicState"/>.
        /// </summary>
        public MusicState? PreviousActiveState { get; }

        /// <summary>
        /// Gets the active <see cref="MusicState"/>.
        /// </summary>
        public MusicState ActiveState { get; }

        /// <summary>
        /// Gets the time in seconds current state is set to be active <see cref="int"/>.
        /// </summary>
        public int KeepStateActive { get; }

        /// <summary>
        /// Gets if song of state should play from start.
        /// </summary>
        public bool PlayFromBegining { get; }
        #endregion
    }
}
