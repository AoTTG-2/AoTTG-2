using Assets.Scripts.Audio;
using Assets.Scripts.Room;
using Assets.Scripts.Services;
using Assets.Scripts.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Audio
{
    /// <summary>
    /// Controls the MusicMixer
    /// </summary>
    public class MusicController : AudioController<MusicController>
    {
        #region PrivateProperties
        private bool firstStart = true;
        private List<AudioSource> audioSources;
        #endregion

        #region PublicProperties
        public List<Playlist> Playlists;
        public MusicState State;
        public float TransitionTime;
        #endregion

        public MusicController() : base() { }

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            audioSources = CreateAudioSources();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            musicService.OnAudioStateChanged += Audio_OnAudioStateChanged;
            SetActivePlaylist(null);
        }

        protected void FixedUpdate()
        {
            StartAudioSourcesIfNotPlaying();
            CheckMusicVolume(Mixer.audioMixer);
            CheckState();
        }
        #endregion

        #region EventListners
        protected void Audio_OnMusicVolumeChanged(object sender, float volume)
        {
            Volume = GetLogVolume(volume);
        }

        private void Audio_OnAudioStateChanged(object sender, MusicState state)
        {
            State = state;
            TransitionToSnapshot(state);
        }

        private void Level_OnLevelLoaded(int scene, Level level)
        {
            //This event is not Invoked as it should, so right now level is always null
            var gamemode = level?.Gamemodes.FirstOrDefault()?.Name;
            var newPlaylist = Playlists.GetByName(level?.SceneName);
            newPlaylist = newPlaylist is null ? Playlists.GetByName(gamemode) : newPlaylist;

            SetActivePlaylist(newPlaylist);
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (!musicService.ActivePlaylist.name.Equals(scene.name))
            {
                Service.Music.SetMusicState(MusicState.Ambient);
            }
            var newPlaylist = Playlists.GetByName(scene.name);
            SetActivePlaylist(newPlaylist);
        }
        #endregion

        #region PrivateMethods
        private void TransitionToSnapshot(MusicState state)
        {
            var snapshot = Mixer.audioMixer.FindSnapshot(state.ToString());

            if (snapshot != null)
            {
                SetCurrentSong();
                snapshot.audioMixer.updateMode = AudioMixerUpdateMode.UnscaledTime;
                snapshot.TransitionTo(TransitionTime);
            }
        }

        private void SetActivePlaylist(Playlist playlist)
        {
            if (playlist is null)
            {
                playlist = Playlists.GetDefault();
            }

            if (!(playlist is null) && !musicService.ActivePlaylist.name.Equals(playlist.name))
            {
                musicService.SetActivePlaylist(playlist);
            }
        }

        private void SetCurrentSong()
        {
            var currentState = musicService.ActiveState;
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(currentState.ToString()));
            var clipName = audioSource.clip?.name;
            var song = musicService.ActivePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(currentState));
            
            if (song != null)
            {
                musicService.SetActiveSong(song);
            }
        }

        private List<AudioSource> CreateAudioSources()
        {
            //Creates one audiosource for each state and sets the outputMixerGroup that has the same name as the state
            List<AudioSource> sources = new List<AudioSource>();
            foreach (var audioState in Enum.GetNames(typeof(MusicState)))
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                var outputs = Mixer.audioMixer.FindMatchingGroups(audioState).ToList();

                audioSource.playOnAwake = false;
                audioSource.outputAudioMixerGroup = outputs.First();

                if (audioState.Equals(MusicState.MainMenu | MusicState.HumanPlayerGrabbed))
                {
                    audioSource.loop = true;
                }

                sources.Add(audioSource);
            }

            return sources;
        }

        private void StartAudioSourcesIfNotPlaying()
        {
            var currentState = musicService.ActiveState;
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
                if (!state.Equals(currentState) && firstStart)
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
                musicService.SetMusicState(State);
            }
        }

        private void CheckMusicVolume(AudioMixer audioMixer)
        {
            // MusicVol is the name of the MusicMixer's exposed volume preoperty
            audioMixer.GetFloat("MusicVol", out var volume);
            var musicVolume = Mathf.Log10(Volume) * 20;

            if (!volume.Equals(musicVolume))
            {
                audioMixer.SetFloat("MusicVol", musicVolume);
                musicService.SetMusicVolume(Volume);
            }
        }
        #endregion
    }
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
