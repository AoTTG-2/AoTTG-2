using Assets.Scripts.Events.Args;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
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
        private readonly IMusicService musicService = Service.Music;
        #endregion

        #region Public Properties
        [Tooltip("Contains the playlists that can be used by this MusicController (playlists should be named the same as the scene they are to be used in).")]
        public List<Playlist> Playlists;
        [Tooltip("Displays the current state of the MusicController, can be changed manually for testing (the system can override any changes you make here, so it's better to use the audiomixer and \"Edit in play mode\").")]
        public MusicState State;
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
            audioSources = CreateAudioSources();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            musicService.OnStateChanged += MusicService_OnStateChanged;
            musicService.OnVolumeChanged += MusicService_OnVolumeChanged; ;
            SetActivePlaylist(null);
        }

        protected void FixedUpdate()
        {
            StartAudiosourcesIfNotPlaying();
            CheckMusicVolume(MixerGroup.audioMixer);
            CheckState();
        }
        #endregion

        #region Eventlistners
        private void MusicService_OnVolumeChanged(MusicVolumeChangedEvent musicVolumeEvent)
        {
            Volume = musicVolumeEvent.Volume;
        }

        private void MusicService_OnStateChanged(MusicStateChangedEvent musicStateEvent)
        {
            State = musicStateEvent.State;
            TransitionToSnapshot(musicStateEvent.State);
        }

        // Use OnLevelLoaded instead when it is working properly
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var newPlaylist = Playlists.GetByName(scene.name);
            SetActivePlaylist(newPlaylist);
        }
        #endregion

        #region Private Methods
        private void TransitionToSnapshot(MusicState state)
        {
            var snapshot = MixerGroup.audioMixer.FindSnapshot(state.ToString());

            if (snapshot != null)
            {
                SetCurrentSong();
                snapshot.audioMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
                snapshot.TransitionTo(TransitionTime);
            }
        }

        private void SetActivePlaylist(Playlist playlist)
        {
            playlist = playlist is null ? Playlists.GetDefault() : playlist;

            if (playlist != null && musicService.ActivePlaylist.name != playlist.name)
            {
                musicService.SetActivePlaylist(new PlaylistChangedEvent(playlist));
            }
        }

        private void SetCurrentSong()
        {
            var currentState = musicService.ActiveState;
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(currentState.ToString()));
            var clipName = audioSource.clip != null ? audioSource.clip.name : null;
            var song = musicService.ActivePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(currentState));
            
            if (song != null)
            {
                musicService.SetActiveSong(new SongChangedEvent(song));
            }
        }

        private List<AudioSource> CreateAudioSources()
        {
            //Creates one audiosource for each state and sets the outputMixerGroup that has the same name as the state
            var sources = new List<AudioSource>();
            foreach (var audioState in Enum.GetNames(typeof(MusicState)))
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                var output = MixerGroup.audioMixer.FindMatchingGroups(audioState).ToList().FirstOrDefault();

                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = output;

                sources.Add(audioSource);
            }

            return sources;
        }

        private void StartAudiosourcesIfNotPlaying()
        {
            audioSources.Where(src => !src.isPlaying).ToList().ForEach(src =>
            {
                var mixerGroupName = src.outputAudioMixerGroup.name;
                var parsed = Enum.TryParse<MusicState>(mixerGroupName, true, out var state);

                if (parsed)
                {
                    var song = musicService.ActivePlaylist.songs.GetRandomByState(state);
                    src.clip = song != null ? song.Clip : null;
                }

                src.volume = 1f;
                if (state != musicService.ActiveState && firstStart)
                {
                    src.PlayDelayed(1);
                }
                else
                {
                    src.Play();
                }
            });

            firstStart = false;
        }

        private void CheckState()
        {
            if (musicService.ActiveState != State)
            {
                musicService.SetMusicState(new MusicStateChangedEvent(State));
            }
        }

        private void CheckMusicVolume(AudioMixer audioMixer)
        {
            audioMixer.GetFloat(VolumeParameterName, out var volume);
            var musicVolume = Volume.Log10Volume();

            if (volume != musicVolume)
            {
                audioMixer.SetFloat(VolumeParameterName, musicVolume);
                musicService.SetMusicVolume(new MusicVolumeChangedEvent(Volume));
            }
        }
        #endregion
    }
}