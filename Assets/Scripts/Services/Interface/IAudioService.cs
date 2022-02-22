using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services.Interface
{
    public interface IAudioService
    {
        /// <summary>
        /// Invoked when a change of audiostate is made
        /// </summary>
        event EventHandler<AudioState> OnAudioStateChanged;
        /// <summary>
        /// Invoked when music volume is changed
        /// </summary>
        event EventHandler<float> OnMusicVolumeChanged;
        event EventHandler<Song> OnSongChanged;

        float GetCurrentMusicVolume();
        AudioState GetCurrentState();
        void InvokeAudioStateChanged(AudioState state);
        void InvokeMusicVolumeChanged(float volume);
        void InvokeSongChanged(Song song);
        string NowPlaying();
        Song GetCurrentSong();
    }
}
