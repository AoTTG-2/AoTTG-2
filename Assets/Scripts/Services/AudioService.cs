using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Services
{
    public class AudioService : IAudioService
    {
        private AudioState currentState;
        private float musicVolume;
        private Song currentSong;
        public event EventHandler<AudioState> OnAudioStateChanged;
        public event EventHandler<float> OnMusicVolumeChanged;
        public event EventHandler<Song> OnSongChanged;

        public void InvokeAudioStateChanged(AudioState state)
        {
            var newState = DeterminateState(state);
            currentState = newState;
            OnAudioStateChanged?.Invoke(this, newState);
        }

        public void InvokeMusicVolumeChanged(float volume)
        {
            musicVolume = volume;
            OnMusicVolumeChanged?.Invoke(this, volume);
        }

        public void InvokeSongChanged(Song song)
        {
            currentSong = song;
            OnSongChanged?.Invoke(this, song);
        }

        public AudioState GetCurrentState()
        {
            return currentState;
        }

        public float GetCurrentMusicVolume()
        {
            return musicVolume;
        }

        public string NowPlaying()
        {
            return $"{currentSong.Name} - {currentSong.Composer}";
        }

        private AudioState DeterminateState(AudioState state)
        {
            return state;
        }

        public Song GetCurrentSong()
        {
            return currentSong;
        }
    }
}
