using Assets.Scripts.Audio;
using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private MusicState activeState;
        private float musicVolume;
        private Song activeSong;
        private Playlist activePlaylist;
        private Dictionary<MusicState, List<MusicState>> stateMatrix;
        private List<MusicState> instantStates;
        private List<MusicState> playFromBegining;
        #endregion

        #region Public Properties
        /// <inheritdoc/>
        public Playlist ActivePlaylist => activePlaylist ?? ScriptableObject.CreateInstance<Playlist>();
        /// <inheritdoc/>
        public MusicState ActiveState => activeState;
        /// <inheritdoc/>
        public float Volume => musicVolume;
        /// <inheritdoc/>
        public string NowPlaying => activeSong is null ? "No song currently playing!" : $"{activeSong.Name} - {activeSong.Composer}";
        #endregion

        #region Events
        /// <inheritdoc/>
        public event OnMusicStateChanged OnStateChanged;
        /// <inheritdoc/>
        public event OnVolumeChanged OnVolumeChanged;
        /// <inheritdoc/>
        public event OnSongChanged OnSongChanged;
        /// <inheritdoc/>
        public event OnPlaylistChanged OnPlaylistChanged;
        #endregion

        #region Constructors
        public MusicService()
        {
            CreateRuleset();
        }
        #endregion

        #region Public Methods
        /// <inheritdoc/>
        public void SetMusicState(MusicStateChangedEvent stateEvent)
        {
            if (ValidateTransition(stateEvent))
            {
                var previousActiveState = activeState;
                activeState = stateEvent.ActiveState;
                var restart = playFromBegining.Contains(activeState);
                var newEvent = new MusicStateChangedEvent(previousActiveState, activeState, stateEvent.KeepStateActive, restart);
                OnStateChanged?.Invoke(newEvent);
            }
        }
        /// <inheritdoc/>
        public void SetMusicVolume(MusicVolumeChangedEvent volumeEvent)
        {
            if (!musicVolume.Equals(volumeEvent.Volume))
            {
                musicVolume = volumeEvent.Volume;
                OnVolumeChanged?.Invoke(volumeEvent);
            }
        }
        /// <inheritdoc/>
        public void SetActiveSong(SongChangedEvent songEvent)
        {
            if (songEvent.Song != null)
            {
                activeSong = songEvent.Song;
                OnSongChanged?.Invoke(songEvent);
            }
        }
        /// <inheritdoc/>
        public void SetActivePlaylist(PlaylistChangedEvent playlistEvent)
        {
            if (playlistEvent.Playlist != null || (activePlaylist != null && activePlaylist.name != playlistEvent.Playlist.name))
            {
                activePlaylist = playlistEvent.Playlist;
                OnPlaylistChanged?.Invoke(playlistEvent);
            }
        }
        #endregion

        #region Private Methods
        private bool ValidateTransition(MusicStateChangedEvent stateEvent)
        {
            var now = DateTime.Now;
            var isTimeToChange = now - nextStateChangeTime < now.TimeOfDay;
            var stateExistsInPlaylist = activePlaylist != null && activePlaylist.songs.GetByState(stateEvent.ActiveState)?.Count > 0;

            // If statematrix contains a key with the current state and any of the values are equal to the incoming
            // state and it has been more than 3 sec (or the time set by the event) since the last change, or it's an instant state,
            // then return true and change state, else return false and don't change state.
            var nextStateNotCurrent = stateEvent.ActiveState != activeState;
            var currentStateExistsAsKeyInMatrix = stateMatrix.TryGetValue(activeState, out var matrixForCurrentState);
            var canTransitionFromCurrentState = currentStateExistsAsKeyInMatrix && (matrixForCurrentState.Any(v => v.Equals(stateEvent.ActiveState)) || matrixForCurrentState.Count < 1);

            var rule = isTimeToChange && nextStateNotCurrent && canTransitionFromCurrentState && stateExistsInPlaylist;
            var exception = instantStates.Contains(stateEvent.ActiveState);

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

        private void CreateRuleset()
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

            playFromBegining = new List<MusicState>() { MusicState.Ambient, MusicState.HumanPlayerGrabbed, MusicState.HumanPlayerDead };
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
