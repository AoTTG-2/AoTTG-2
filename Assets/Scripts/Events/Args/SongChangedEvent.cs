using Assets.Scripts.Audio;
using System;

namespace Assets.Scripts.Events.Args
{
    public class SongChangedEvent : EventArgs
    {
        #region Constructors
        public SongChangedEvent(Song song)
        {
            Song = song;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the active <see cref="Song"/>
        /// </summary>
        public Song Song { get; }
        #endregion
    }
}
