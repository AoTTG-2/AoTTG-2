using Assets.Scripts.Audio;

namespace Assets.Scripts.Events.Args
{
    public class PlaylistChangedEvent
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
