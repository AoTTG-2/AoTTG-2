using Assets.Scripts.Events;
using Assets.Scripts.Events.Args;
using Assets.Scripts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Controls the music.
    /// </summary>
    public class MusicController : AudioController<MusicController>
    {
        #region Private Properties
        private bool firstStart = true;
        private DateTime nextStateChangeTime;
        private Song activeSong;
        private Playlist activePlaylist;
        private Dictionary<MusicState, List<MusicState>> stateMatrix;
        private List<MusicState> instantStates;
        private bool isPaused;
        #endregion

        #region Public Properties
        public MusicState ActiveState;
        [Tooltip("Contains the playlists that can be used by this MusicController (playlists should be named the same as the scene they are to be used in).")]
        public List<Playlist> Playlists;
        [Tooltip("The time in seconds for transitioning from one snapshot to another.")]
        public float TransitionTime;
        /// <summary>
        /// Gets the name of the current song and composer.
        /// </summary>
        public string NowPlaying => activeSong != null ? $"{activeSong.Name} - {activeSong.Composer}" : string.Empty;
        #endregion

        #region Events
        /// <summary>
        /// The active <see cref="MusicState"/> has changed.
        /// </summary>
        public event OnMusicStateChanged OnStateChanged;
        /// <summary>
        /// The active <see cref="Song"/> has changed.
        /// </summary>
        public event OnSongChanged OnSongChanged;
        /// <summary>
        /// The active <see cref="Playlist"/> has changed.
        /// </summary>
        public event OnPlaylistChanged OnPlaylistChanged;
        /// <summary>
        /// The volume has changed.
        /// </summary>
        public event OnVolumeChanged OnVolumeChanged;
        #endregion

        #region Constructors
        public MusicController() : base() { }
        #endregion

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            CreateAudioSources();
            BuildStateMatrix();
            SetActivePlaylist(null);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Service.Pause.OnPaused += Pause_OnPaused;
            Service.Pause.OnUnPaused += Pause_OnUnPaused;
        }

        protected void FixedUpdate()
        {
            StartAudiosourcesIfNotPlaying();
            CheckMusicVolume(MixerGroup.audioMixer);
        }
        #endregion

        #region Eventlistners
        // Use OnLevelLoaded instead when it is working properly
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var newPlaylist = Playlists.GetByName(scene.name);
            SetActivePlaylist(newPlaylist);
        }
        private void Pause_OnPaused(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void Pause_OnUnPaused(object sender, EventArgs e)
        {
            TogglePause();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the music volume.
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(float volume)
        {
            Volume = NormalizeVolume(volume);
            OnVolumeChanged?.Invoke(new MusicVolumeChangedEvent(Volume));
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the active <see cref="MusicState"/>.
        /// </summary>
        /// <param name="musicStateEvent"></param>
        public void SetMusicState(MusicStateChangedEvent musicStateEvent)
        {
            if (ValidateTransition(musicStateEvent))
            {
                ActiveState = musicStateEvent.State;
                OnStateChanged?.Invoke(musicStateEvent);
                TransitionToSnapshot(musicStateEvent.State);
            }
        }
        #endregion

        #region Private Methods
        private void TogglePause()
        {
            audioSources.ForEach(src =>
            {
                src.volume = Volume;
                if (isPaused)
                {
                    src.volume = Volume;
                    src.UnPause();
                }
                else
                {
                    src.volume = 0;
                    src.Pause();
                }

            });
            isPaused = !isPaused;
        }

        private void TransitionToSnapshot(MusicState state)
        {
            var snapshot = MixerGroup.audioMixer.FindSnapshot(state.ToString());

            if (snapshot != null)
            {
                SetActiveSong();
                snapshot.audioMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
                snapshot.TransitionTo(TransitionTime);
            }
        }

        private void SetActivePlaylist(Playlist playlist)
        {
            playlist = playlist is null ? Playlists.GetDefault() : playlist;

            if (playlist != null && activePlaylist?.name != playlist.name)
            {
                activePlaylist = playlist;
                OnPlaylistChanged?.Invoke(new PlaylistChangedEvent(playlist));
            }
        }

        private void SetActiveSong()
        {
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(ActiveState.ToString()));
            var clipName = audioSource.clip != null ? audioSource.clip.name : null;
            var song = activePlaylist != null ? activePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(ActiveState)) : null;
            
            if (song != null)
            {
                activeSong = song;
                OnSongChanged?.Invoke(new SongChangedEvent(song));
            }
        }

        private void CreateAudioSources()
        {
            //Creates one audiosource for each state and sets the outputMixerGroup that has the same name as the state
            foreach (var audioState in Enum.GetNames(typeof(MusicState)))
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                var output = MixerGroup.audioMixer.FindMatchingGroups(audioState).ToList().FirstOrDefault();

                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = output;

                audioSources.Add(audioSource);
            }
        }

        private void StartAudiosourcesIfNotPlaying()
        {
            audioSources.Where(src => !src.isPlaying).ToList().ForEach(src =>
            {
                var mixerGroupName = src.outputAudioMixerGroup.name;
                var parsed = Enum.TryParse<MusicState>(mixerGroupName, true, out var state);

                if (parsed)
                {
                    var song = activePlaylist.songs.GetRandomByState(state);
                    src.clip = song != null ? song.Clip : null;
                }

                src.volume = 1f;
                if (state != ActiveState && firstStart)
                {
                    src.PlayDelayed(1);
                }
                else if (!isPaused)
                {
                    src.Play();
                }
            });

            firstStart = false;
        }

        private void CheckMusicVolume(AudioMixer audioMixer)
        {
            audioMixer.GetFloat(VolumeParameterName, out var mixerVolume);
            var musicVolume = Volume.Log10Volume();

            if (mixerVolume != musicVolume)
            {
                audioMixer.SetFloat(VolumeParameterName, musicVolume);
            }
        }

        private bool ValidateTransition(MusicStateChangedEvent stateEvent)
        {
            var now = DateTime.Now;
            var isTimeToChange = now - nextStateChangeTime < now.TimeOfDay;
            var stateExistsInPlaylist = activePlaylist != null && activePlaylist.songs.GetByState(stateEvent.State)?.Count > 0;

            var nextStateNotCurrent = stateEvent.State != ActiveState;
            var currentStateExistsAsKeyInMatrix = stateMatrix.TryGetValue(ActiveState, out var matrixForCurrentState);
            var canTransitionFromCurrentState = currentStateExistsAsKeyInMatrix && (matrixForCurrentState.Any(v => v.Equals(stateEvent.State)) || matrixForCurrentState.Count < 1);
            var rule = isTimeToChange && nextStateNotCurrent && canTransitionFromCurrentState && stateExistsInPlaylist;
            var exception = instantStates.Contains(stateEvent.State);

            // If statematrix contains a key with the current state   
            // and any of the values for that key are equal to the incoming state
            // and the time to to change has passed, or it's an instant state,
            // then set next state change time and return true, else return false.
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
            // The statematrix is a ruleset saying that if the current state = key
            // then value is the possible states we can transition to.
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