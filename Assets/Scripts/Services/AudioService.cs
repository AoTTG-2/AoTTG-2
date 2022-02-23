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
        private DateTime latestStateChange;
        private AudioState? currentState;
        private float musicVolume;
        private Song currentSong;
        public event EventHandler<AudioState> OnAudioStateChanged;
        public event EventHandler<float> OnMusicVolumeChanged;
        public event EventHandler<Song> OnSongChanged;

        public void InvokeAudioStateChanged(AudioState state)
        {
            var newState = DeterminateState(state);
            if (newState is null)
            {
                return;
            }
            currentState = newState.Value;
            OnAudioStateChanged?.Invoke(this, newState.Value);
        }

        public void InvokeMusicVolumeChanged(float volume)
        {
            if (musicVolume.Equals(volume))
            {
                return;
            }

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
            return currentState is null ? AudioState.MainMenu : currentState.Value;
        }

        public float GetCurrentMusicVolume()
        {
            return musicVolume;
        }

        public string NowPlaying()
        {
            return $"{currentSong.Name} - {currentSong.Composer}";
        }

        private AudioState? DeterminateState(AudioState state)
        {
            var timeToChange = DateTime.Now - latestStateChange > new TimeSpan(0, 0, 5);

            var stateMatrix = new Dictionary<AudioState, List<AudioState>>
            {
                { AudioState.MainMenu, new List<AudioState>() { AudioState.Ambient, AudioState.Action, AudioState.Neutral } },
                { AudioState.Ambient, new List<AudioState>() { AudioState.Neutral, AudioState.Combat, AudioState.Action } },
                { AudioState.Neutral, new List<AudioState>() { AudioState.Ambient, AudioState.Combat, AudioState.Action } },
                { AudioState.Action, new List<AudioState>() { AudioState.Combat, AudioState.Neutral } },
                { AudioState.Combat, new List<AudioState>() { AudioState.Neutral } },
            };

            if (state.Equals(AudioState.MainMenu))
            {
                return state;
            }

            if (state.Equals(currentState))
            {
                return null;
            }

            //If statematrix contains a key with the current state and any of the values are equal to the incoming
            //state and it has been more than 5 sec since the last change then change state, else return null and don't change state
            if (stateMatrix.Any(vkp => vkp.Key.Equals(currentState) && vkp.Value.Any(v => v.Equals(state))) && timeToChange)
            {
                latestStateChange = DateTime.Now;
                return state;
            }
            else
            {
                return null;
            }
        }

        public Song GetCurrentSong()
        {
            return currentSong;
        }
    }
}
