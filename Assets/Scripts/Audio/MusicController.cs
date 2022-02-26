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
    public class MusicController : AudioController
    {
        #region Private Properties
        private bool firstStart = true;
        private bool isPaused;
        #endregion

        #region Public Properties
        public MusicState ActiveState;
        [Tooltip("Contains the playlists that can be used by this MusicController (playlists should be named the same as the scene they are to be used in).")]
        public List<Playlist> Playlists;
        [Tooltip("The time in seconds for transitioning from one snapshot to another.")]
        public float TransitionTime;
        #endregion

        #region Constructors
        public MusicController() : base() { }
        #endregion

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            CreateAudioSources();
            SetActivePlaylist(null);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            Service.Pause.OnPaused += Pause_OnPaused;
            Service.Pause.OnUnPaused += Pause_OnUnPaused;
            Service.Music.OnStateChanged += Music_OnStateChanged;
            Service.Music.OnVolumeChanged += Music_OnVolumeChanged;
        }


        protected void FixedUpdate()
        {
            StartAudiosourcesIfNotPlaying();
            CheckMusicVolume(MixerGroup.audioMixer);
        }
        #endregion

        #region Eventlistners
        private void Music_OnVolumeChanged(MusicVolumeChangedEvent musicVolumeEvent)
        {
            Volume = NormalizeVolume(musicVolumeEvent.Volume);
        }

        private void Music_OnStateChanged(MusicStateChangedEvent musicStateEvent)
        {
            ActiveState = musicStateEvent.State;
            TransitionToSnapshot(musicStateEvent.State);
        }

        // Use OnLevelLoaded instead, when it is working properly
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
            Service.Music.SetActivePlaylist(new PlaylistChangedEvent(playlist));
        }

        private void SetActiveSong()
        {
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(ActiveState.ToString()));
            var clipName = audioSource.clip != null ? audioSource.clip.name : null;
            var song = Service.Music.ActivePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(ActiveState));
            Service.Music.SetActiveSong(new SongChangedEvent(song));
        }

        private void CreateAudioSources()
        {
            //Creates one audiosource for each state and sets the outputMixerGroup that has the same name as the state
            foreach (var audioState in Enum.GetNames(typeof(MusicState)))
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                var output = MixerGroup.audioMixer.FindMatchingGroups(audioState).AsEnumerable().FirstOrDefault();

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
                Song song = null;

                if (parsed)
                {
                    song = Service.Music.ActivePlaylist.songs.GetRandomByState(state);
                    src.clip = song != null ? song.Clip : null;
                }

                if (ActiveState == state && src.clip != null)
                {
                    Service.Music.SetActiveSong(new SongChangedEvent(song));
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
        #endregion
    }

    public enum MusicState
    {
        MainMenu,
        Combat,
        Neutral,
        Ambient,
        Action,
        HumanPlayerDead,
        HumanPlayerGrabbed,
    }
}