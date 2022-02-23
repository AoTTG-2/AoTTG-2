using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services.Interface
{
    public interface IMusicService : IAudioService
    {
        Playlist ActivePlaylist { get; }
        MusicState ActiveState { get; }
        string NowPlaying { get; }

        event EventHandler<MusicState> OnAudioStateChanged;
        event EventHandler<float> OnMusicVolumeChanged;
        event EventHandler<Song> OnSongChanged;
        event EventHandler<Playlist> OnPlaylistChanged;

        void SetMusicState(MusicState state);
        void SetMusicVolume(float volume);
        void SetActiveSong(Song song);
        void SetActivePlaylist(Playlist playlist);
    }
}
