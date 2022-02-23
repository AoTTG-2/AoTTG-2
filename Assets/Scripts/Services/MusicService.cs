using Assets.Scripts.Audio;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class MusicService : IMusicService
    {
        private DateTime latestStateChange;
        private MusicState currentState;
        private float musicVolume;
        private Song currentSong;
        private Playlist currentPlaylist;
        private Dictionary<MusicState, List<MusicState>> stateMatrix;
        private List<MusicState> instantStates;
        private bool stateIsNull;

        public event EventHandler<MusicState> OnAudioStateChanged;
        public event EventHandler<float> OnMusicVolumeChanged;
        public event EventHandler<Song> OnSongChanged;
        public event EventHandler<Playlist> OnPlaylistChanged;
        public Playlist ActivePlaylist => currentPlaylist is null ? ScriptableObject.CreateInstance<Playlist>() : currentPlaylist;

        public MusicState ActiveState => currentState;

        public float Volume => musicVolume;

        public string NowPlaying => currentSong is null ? "No song currently playing!" : $"{currentSong?.Name} - {currentSong.Composer}";

        public MusicService()
        {
            stateIsNull = true;
            BuildStateMatrix();
        }

        public void SetMusicState(MusicState state)
        {
            var newState = DeterminateState(state);
            if (newState is null)
            {
                return;
            }
            currentState = newState.Value;
            OnAudioStateChanged?.Invoke(this, newState.Value);
        }

        public void SetMusicVolume(float volume)
        {
            if (musicVolume.Equals(volume))
            {
                return;
            }

            musicVolume = volume;
            OnMusicVolumeChanged?.Invoke(this, volume);
        }

        public void SetActiveSong(Song song)
        {
            currentSong = song;
            OnSongChanged?.Invoke(this, song);
        }

        public void SetActivePlaylist(Playlist playlist)
        {
            currentPlaylist = playlist;
            OnPlaylistChanged?.Invoke(this, playlist);
        }

        // Ruleset for state transitions
        private MusicState? DeterminateState(MusicState nextState)
        {
            // Used as a hack to not trigger anything til the grab animation is over.
            var standardBlockTime = MusicState.HumanPlayerGrabbed.Equals(currentState) ? 6 : 3;
            var timeToChange = DateTime.Now - latestStateChange > new TimeSpan(0, 0, standardBlockTime);
            var stateExistsInPlaylist = currentPlaylist.songs.GetByState(nextState)?.Count > 0;

            // If statematrix contains a key with the current state and any of the values are equal to the incoming
            // state and it has been more than 5 sec since the last change, or it's an instant state, then change state,
            // else return null and don't change state.
            var matrixForCurrentState = new List<MusicState>();
            var nextStateNotCurrent = !nextState.Equals(currentState);
            var currentStateAsKeyInMatrix = stateMatrix.TryGetValue(currentState, out matrixForCurrentState);
            var canTransitionFromCurrentState = matrixForCurrentState != null && (matrixForCurrentState.Any(v => v.Equals(nextState)) || matrixForCurrentState.Count < 1);
            var rule = nextStateNotCurrent && currentStateAsKeyInMatrix && canTransitionFromCurrentState && timeToChange && stateExistsInPlaylist;
            var exception = instantStates.Contains(nextState);
            if (rule || exception)
            {
                SetLastestStateChange();
                return nextState;
            }
            else
            {
                return null;
            }
        }

        private void BuildStateMatrix()
        {
            // The statematrix is a ruleset saying that if the current state = key, value is the possible states we can transition to.
            // empty list in value = accept all transitions
            stateMatrix = new Dictionary<MusicState, List<MusicState>>
            {
                { MusicState.MainMenu, new List<MusicState>() },
                { MusicState.Ambient, new List<MusicState>() { MusicState.Neutral, MusicState.Combat, MusicState.Action } },
                { MusicState.Neutral, new List<MusicState>() { MusicState.Ambient, MusicState.Combat, MusicState.Action } },
                { MusicState.Action, new List<MusicState>() { MusicState.Combat, MusicState.Neutral } },
                { MusicState.Combat, new List<MusicState>() { MusicState.Neutral } },
                { MusicState.HumanPlayerDead, new List<MusicState>() { MusicState.Ambient } },
            };

            // These states are exceptions from the stateMatrix and will always trigger a change
            instantStates = new List<MusicState>() { MusicState.MainMenu, MusicState.HumanPlayerGrabbed, MusicState.HumanPlayerDead };
        }

        private void SetLastestStateChange()
        {
            latestStateChange = DateTime.Now;
        } 
    }
}
