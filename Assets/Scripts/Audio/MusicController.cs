using Assets.Scripts.Events.Args;
using Assets.Scripts.Services;
using System;
using System.Collections;
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
    internal class MusicController : AudioController
    {
        #region Private Properties
        private bool firstStart = true;
        private bool isPaused;
        [SerializeField]
        [Tooltip("Changes the active MusicState, used for testing transitions (Still abides by the internal transition rules).")]
        private MusicState activeState;
        [SerializeField]
        [Tooltip("Contains the playlists that can be used by this MusicController (playlists should be named the same as the scene they are to be used in).")]
        private List<Playlist> playlists;
        [SerializeField]
        [Tooltip("The time in seconds for transitioning from one snapshot to another.")]
        private float transitionTime;
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


        private void FixedUpdate()
        {
            StartAudioSourcesIfNotPlaying();
            SyncVolumeFromEditor();
            SyncStateForEditor();
        }
        #endregion

        #region Eventlistners
        private void Music_OnVolumeChanged(MusicVolumeChangedEvent musicVolumeEvent)
        {
            volume = NormalizeVolume(musicVolumeEvent.Volume);
        }

        private void Music_OnStateChanged(MusicStateChangedEvent musicStateEvent)
        {

            activeState = musicStateEvent.ActiveState;
            CrossfadeVolume(musicStateEvent);
        }

        // Use OnLevelLoaded instead, when it is working properly
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var newPlaylist = playlists.GetByName(scene.name);
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
                if (isPaused)
                {
                    src.volume = MaxVolume;
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

        private void SetActivePlaylist(Playlist playlist)
        {
            playlist ??= playlists.GetDefault();
            Service.Music.SetActivePlaylist(new PlaylistChangedEvent(playlist));
        }

        private void SetActiveSong()
        {
            var state = Service.Music.ActiveState;
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(state.ToString()));
            var clipName = audioSource?.clip != null ? audioSource.clip.name : null;
            var song = Service.Music.ActivePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(state));
            Service.Music.SetActiveSong(new SongChangedEvent(song));
        }

        private void CreateAudioSources()
        {
            //Creates one audiosource for each state and sets the outputMixerGroup that has the same name as the state
            foreach (var audioState in Enum.GetNames(typeof(MusicState)))
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                var output = mixerGroup.audioMixer.FindMatchingGroups(audioState).AsEnumerable().FirstOrDefault();

                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = output;

                audioSources.Add(audioSource);
            }
        }

        private void StartAudioSourcesIfNotPlaying()
        {
            var serviceState = Service.Music.ActiveState;
            audioSources.Where(src => !src.isPlaying).ToList().ForEach(src =>
            {
                var mixerGroupName = src.outputAudioMixerGroup.name;
                var parsed = Enum.TryParse<MusicState>(mixerGroupName, true, out var mixerState);
                Song song = null;

                if (parsed)
                {
                    song = Service.Music.ActivePlaylist.songs.GetRandomByState(mixerState);
                    src.clip = song != null ? song.Clip : null;
                }

                if (serviceState == mixerState && src.clip != null)
                {
                    Service.Music.SetActiveSong(new SongChangedEvent(song));
                }

                src.volume = MaxVolume;
                if (mixerState != serviceState && firstStart)
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

        private void SyncVolumeFromEditor()
        {
            var mixer = mixerGroup.audioMixer;
            var exposedParameterName = GetExposedParameterName(mixerGroup);
            mixer.GetFloat(exposedParameterName, out var mixerVolume);
            var musicVolume = volume.ToLogVolume();

            if (mixerVolume != musicVolume)
            {

                mixer.SetFloat(exposedParameterName, musicVolume);
            }
        }

        private void SyncStateForEditor()
        {
            if (activeState != Service.Music.ActiveState)
            {
                Service.Music.SetMusicState(new MusicStateChangedEvent(activeState));
            }
        }

        /// <summary>
        /// our custom crossfade uses exposed variables from the audio mixer groups to 
        /// perform a linear crossfade between diffrent groups, this is technically a hack
        /// since Unity only supports linear crossfading of the output volume through the much simpler to use snapshot mode,
        /// but on the logarithmic volume scale where -40db is 50% which creats a short period of silence in the middle of the fade,
        /// this is a better way to do it since there is no silence, even if it's more complicated.
        /// </summary>
        /// <param name="stateChangedEvent"></param>
        private void CrossfadeVolume(MusicStateChangedEvent stateChangedEvent)
        {
            var from = stateChangedEvent.PreviousActiveState;
            var to = stateChangedEvent.ActiveState;

            if (stateChangedEvent.PlayFromBegining)
            {
                PlayClipFromBegining(stateChangedEvent.ActiveState);
            }

            SetActiveSong();

            var fromMixerGroup = mixerGroup.audioMixer.FindMatchingGroups(from.ToString()).FirstOrDefault();
            var toMixerGroup = mixerGroup.audioMixer.FindMatchingGroups(to.ToString()).FirstOrDefault();

            StartCoroutine(CrossfadeBetweenGroups(fromMixerGroup, toMixerGroup));
        }

        private void PlayClipFromBegining(MusicState state)
        {
            var source = audioSources.First(src => src.outputAudioMixerGroup.name.Equals(state.ToString()));
            source.Stop();
            source.Play();
        }

        private IEnumerator CrossfadeBetweenGroups(AudioMixerGroup from, AudioMixerGroup to)
        {
            float timeElapsed = 0;

            while (timeElapsed < transitionTime)
            {
                var function = timeElapsed / transitionTime;
                var fromVol = Mathf.Lerp(MaxVolume, MinVolume, function).ToLogVolume();
                var toVol = Mathf.Lerp(MinVolume, MaxVolume, function).ToLogVolume();
                FadeGroup(from, fromVol);
                FadeGroup(to, toVol);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            if (from != to)
            {
                // the loop above doesn't bring the volume quite all the way too zero, so we do that here
                from.audioMixer.SetFloat(GetExposedParameterName(from), MinVolume.ToLogVolume());
            }
        }

        private void FadeGroup(AudioMixerGroup group, float groupVol)
        {
            if (group != null)
            {
                group.audioMixer.SetFloat(GetExposedParameterName(group), groupVol);
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