using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services.Interface
{
    public interface IMusicService : IAudioService
    {
        #region Properties
        Playlist ActivePlaylist { get; }
        MusicState ActiveState { get; }
        string NowPlaying { get; }
        #endregion


        #region Events
        event OnMusicStateChanged OnStateChanged;
        event OnVolumeChanged OnVolumeChanged;
        event OnSongChanged OnSongChanged;
        event OnPlaylistChanged OnPlaylistChanged;
        #endregion


        #region Methods
        void SetMusicState(MusicStateChangedEvent stateEvent);
        void SetMusicVolume(MusicVolumeChangedEvent volumeEvent);
        void SetActiveSong(SongChangedEvent songEvent);
        void SetActivePlaylist(PlaylistChangedEvent playlistEvent);
        #endregion
    }
}
