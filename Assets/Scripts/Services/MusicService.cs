using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// Services that stores states and handles events related to the music.
    /// </summary>
    public class MusicService : IMusicService
    {
        #region Private Properties
        private DateTime nextStateChangeTime;
        private MusicState currentState;
        private float musicVolume;
        private Song currentSong;
        private Playlist currentPlaylist;
        private Dictionary<MusicState, List<MusicState>> stateMatrix;
        private List<MusicState> instantStates;
        #endregion

        #region Events
        /// <summary>
        /// The active <see cref="MusicState"/> has changed.
        /// </summary>
        public event OnMusicStateChanged OnStateChanged;
        /// <summary>
        /// The music volume has changed.
        /// </summary>
        public event OnVolumeChanged OnVolumeChanged;
        /// <summary>
        /// The active <see cref="Song"/> has changed.
        /// </summary>
        public event OnSongChanged OnSongChanged;
        /// <summary>
        /// The active <see cref="Playlist"/> has changed.
        /// </summary>
        public event OnPlaylistChanged OnPlaylistChanged;
        #endregion
        
        #region Public Properties
        /// <summary>
        /// Gets the currently active <see cref="Playlist"/>
        /// </summary>
        public Playlist ActivePlaylist => currentPlaylist is null ? ScriptableObject.CreateInstance<Playlist>() : currentPlaylist;
        /// <summary>
        /// Gets the currently active <see cref="MusicState"/>
        /// </summary>
        public MusicState ActiveState => currentState;
        /// <summary>
        /// Gets the current music volume.
        /// </summary>
        public float Volume => musicVolume;
        /// <summary>
        /// Gets name and composer for the <see cref="Song"/> that is currently playing.
        /// </summary>
        public string NowPlaying => currentSong is null ? "No song currently playing!" : $"{currentSong.Name} - {currentSong.Composer}";
        #endregion

        #region Constructors
        public MusicService()
        {
            BuildStateMatrix();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the active <see cref="MusicState"/>.
        /// </summary>
        /// <param name="stateEvent"></param>
        public void SetMusicState(MusicStateChangedEvent stateEvent)
        {
            if (!ValidateTransition(stateEvent))
            {
                return;
            }

            currentState = stateEvent.State;
            OnStateChanged?.Invoke(stateEvent);
        }
        /// <summary>
        /// Sets the music volume.
        /// </summary>
        /// <param name="volumeEvent"></param>
        public void SetMusicVolume(MusicVolumeChangedEvent volumeEvent)
        {
            if (musicVolume.Equals(volumeEvent.Volume))
            {
                return;
            }

            musicVolume = volumeEvent.Volume;
            OnVolumeChanged?.Invoke(volumeEvent);
        }
        /// <summary>
        /// Sets the active <see cref="Song"/>.
        /// </summary>
        /// <param name="songEvent"></param>
        public void SetActiveSong(SongChangedEvent songEvent)
        {
            currentSong = songEvent.Song;
            OnSongChanged?.Invoke(songEvent);
        }
        /// <summary>
        /// Sets the active <see cref="Playlist"/>.
        /// </summary>
        /// <param name="playlistEvent"></param>
        public void SetActivePlaylist(PlaylistChangedEvent playlistEvent)
        {
            currentPlaylist = playlistEvent.Playlist;
            OnPlaylistChanged?.Invoke(playlistEvent);
        }
        #endregion

        #region Private Methods
        private bool ValidateTransition(MusicStateChangedEvent stateEvent)
        {
            var now = DateTime.Now;
            var isTimeToChange = now - nextStateChangeTime < now.TimeOfDay;
            var stateExistsInPlaylist = currentPlaylist.songs.GetByState(stateEvent.State)?.Count > 0;

            // If statematrix contains a key with the current state and any of the values are equal to the incoming
            // state and it has been more than 3 sec (or the time set by the event) since the last change, or it's an instant state,
            // then return true and change state, else return false and don't change state.
            var nextStateNotCurrent = stateEvent.State != currentState;
            var currentStateExistsAsKeyInMatrix = stateMatrix.TryGetValue(currentState, out var matrixForCurrentState);
            var canTransitionFromCurrentState = currentStateExistsAsKeyInMatrix && (matrixForCurrentState.Any(v => v.Equals(stateEvent.State)) || matrixForCurrentState.Count < 1);

            var rule = isTimeToChange && nextStateNotCurrent && canTransitionFromCurrentState && stateExistsInPlaylist;
            var exception = instantStates.Contains(stateEvent.State);

            if (rule || exception)
            {
                SetNextStateChangeTime(stateEvent);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BuildStateMatrix()
        {
            // The statematrix is a ruleset saying that if the current state = key then value is the possible states we can transition to.
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

        private void SetNextStateChangeTime(MusicStateChangedEvent stateEvent)
        {
            var seconds = stateEvent.KeepStateActive.Equals(0) ? 3 : stateEvent.KeepStateActive;
            // sets a point in time when the next state transition can take place
            nextStateChangeTime = DateTime.Now.AddSeconds(seconds);
        }
        #endregion
    }
}
