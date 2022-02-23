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
    public class AudioController : SingeltonMonoBehaviour<AudioController>
    {
        #region PrivateProperties
        private float minVolume = 0.0001f;
        private float maxVolume = 1;
        private bool firstStart = true;
        private Playlist activePlaylist;
        private readonly IAudioService audioService = Service.Audio;
        private List<AudioSource> audioSources;
        #endregion

        #region PublicProperties
        public AudioMixerGroup mixer;
        public List<Playlist> Playlists;
        public AudioState State;
        [Range(0.0001f, 1f)]
        public float MusicVolume = 0.5f;
        public float TransitionTime = 1.5f;
        #endregion

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            audioSources = gameObject.GetComponents<AudioSource>().ToList();
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            audioService.OnAudioStateChanged += Audio_OnAudioStateChanged;
            audioService.OnMusicVolumeChanged += Audio_OnMusicVolumeChanged;
            SetActivePlaylist(null);
        }

        protected void FixedUpdate()
        {
            StartAudioSourcesIfNotPlaying();
            CheckVolume();
            CheckState();
        }

        private void StartAudioSourcesIfNotPlaying()
        {
            var currentState = audioService.GetCurrentState();
            audioSources.Where(src => !src.isPlaying).ToList().ForEach(src =>
            {
                var mixerGroupName = src.outputAudioMixerGroup.name;
                var parsed = Enum.TryParse<AudioState>(mixerGroupName, true, out var state);

                if (parsed)
                {
                    var song = activePlaylist.songs.GetRandomByState(state);
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

        private void CheckVolume()
        {
            mixer.audioMixer.GetFloat("MusicVol", out var volume);
            var musicVolume = Mathf.Log10(MusicVolume) * 20;

            if (!volume.Equals(musicVolume))
            {
                mixer.audioMixer.SetFloat("MusicVol", musicVolume);
                audioService.InvokeMusicVolumeChanged(MusicVolume);
            }
        }

        private void CheckState()
        {
            if (audioService.GetCurrentState() != State)
            {
                audioService.InvokeAudioStateChanged(State);
            }
        }
        #endregion

        #region EventListners
        private void Audio_OnMusicVolumeChanged(object sender, float volume)
        {
            MusicVolume = volume == 0 ? minVolume : volume > 1 ? maxVolume : volume;
        }

        private void Audio_OnAudioStateChanged(object sender, AudioState state)
        {
            State = state;
            TransitionToSnapshot(state);
        }

        private void Level_OnLevelLoaded(int scene, Level level)
        {
            //This event is not Invoked as it should, so now level is always null
            var gamemode = level?.Gamemodes.FirstOrDefault()?.Name;
            var newPlaylist = Playlists.GetByName(level?.SceneName);
            newPlaylist = newPlaylist is null ? Playlists.GetByName(gamemode) : newPlaylist;

            SetActivePlaylist(newPlaylist);
        }

        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (activePlaylist != null && !activePlaylist.name.Equals(scene.name))
            {
                Service.Audio.InvokeAudioStateChanged(AudioState.Ambient);
            }
            var newPlaylist = Playlists.GetByName(scene.name);
            SetActivePlaylist(newPlaylist);
        }
        #endregion

        #region PrivateMethods
        private void TransitionToSnapshot(AudioState state)
        {
            var snapshot = mixer.audioMixer.FindSnapshot(state.ToString());

            if (snapshot != null && )
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

            if (!(playlist is null) && (activePlaylist is null || !activePlaylist.name.Equals(playlist.name)))
            {
                activePlaylist = playlist;
                TransitionToSnapshot(audioService.GetCurrentState());
            }
        }

        private void SetCurrentSong()
        {
            var currentState = audioService.GetCurrentState();
            var audioSource = audioSources.FirstOrDefault(src => src.outputAudioMixerGroup.name.Equals(currentState.ToString()));
            var clipName = audioSource.clip?.name;
            var song = activePlaylist.songs.FirstOrDefault(s => s.Name.Equals(clipName) && s.Type.Equals(currentState));
            
            if (song != null)
            {
                audioService.InvokeSongChanged(song);
            }
        }
        #endregion
    }
}

public enum AudioState
{
    MainMenu,
    Combat,
    Neutral,
    Ambient,
    Action
}
