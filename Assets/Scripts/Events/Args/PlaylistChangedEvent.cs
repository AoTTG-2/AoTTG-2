using Assets.Scripts.Audio;
using System;

namespace Assets.Scripts.Events.Args
{
    public class PlaylistChangedEvent : EventArgs
    {
        #region Constructors
        public PlaylistChangedEvent(Playlist playlist)
        {
            Playlist = playlist;
        }
        #endregion

        #region Public Properties
        public Playlist Playlist { get; }
        #endregion
    }
}
