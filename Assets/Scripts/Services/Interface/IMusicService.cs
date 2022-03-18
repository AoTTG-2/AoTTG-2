using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;

namespace Assets.Scripts.Services.Interface
{
    public interface IMusicService
    {
        #region Properties
        /// <summary>
        /// Gets the currently active <see cref="Playlist"/>
        /// </summary>
        Playlist ActivePlaylist { get; }
        /// <summary>
        /// Gets the currently active <see cref="MusicState"/>
        /// </summary>
        MusicState ActiveState { get; }
        /// <summary>
        /// Gets the current music volume.
        /// </summary>
        float Volume { get; }
        /// <summary>
        /// Gets name and composer for the <see cref="Song"/> that is currently playing.
        /// </summary>
        string NowPlaying { get; }
        #endregion


        #region Events
        /// <summary>
        /// The active <see cref="MusicState"/> has changed.
        /// </summary>
        event OnMusicStateChanged OnStateChanged;
        /// <summary>
        /// The music volume has changed.
        /// </summary>
        event OnVolumeChanged OnVolumeChanged;
        /// <summary>
        /// The active <see cref="Song"/> has changed.
        /// </summary>
        event OnSongChanged OnSongChanged;
        /// <summary>
        /// The active <see cref="Playlist"/> has changed.
        /// </summary>
        event OnPlaylistChanged OnPlaylistChanged;
        #endregion


        #region Methods
        /// <summary>
        /// Sets the active <see cref="MusicState"/>.
        /// </summary>
        /// <param name="stateEvent"></param>
        void SetMusicState(MusicStateChangedEvent stateEvent);
        /// <summary>
        /// Sets the music volume.
        /// </summary>
        /// <param name="volumeEvent"></param>
        void SetMusicVolume(MusicVolumeChangedEvent volumeEvent);
        /// <summary>
        /// Sets the active <see cref="Song"/>.
        /// </summary>
        /// <param name="songEvent"></param>
        void SetActiveSong(SongChangedEvent songEvent);
        /// <summary>
        /// Sets the active <see cref="Playlist"/>.
        /// </summary>
        /// <param name="playlistEvent"></param>
        void SetActivePlaylist(PlaylistChangedEvent playlistEvent);
        #endregion
    }
}
