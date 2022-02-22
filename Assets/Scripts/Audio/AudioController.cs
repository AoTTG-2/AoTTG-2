using Assets.Scripts.Audio;
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

public class AudioController : SingeltonMonoBehaviour<AudioController>
{
    #region PrivateProperties
    private Playlist activePlaylist;
    private Song activeSong;
    private readonly IAudioService audioService = Service.Audio;
    #endregion

    #region PublicProperties
    public AudioSource music01, music02;
    public List<Playlist> Playlists;
    public AudioState State;
    [Range(0f, 1f)]
    public float MusicVolume = 0.5f;
    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
        Service.Level.OnLevelLoaded += Level_OnLevelLoaded;
        audioService.OnAudioStateChanged += Audio_OnAudioStateChanged;
        audioService.OnMusicVolumeChanged += Audio_OnMusicVolumeChanged;
        audioService.OnSongChanged += AudioService_OnSongChanged;
        SetActivePlaylist(null);
    }

    protected void FixedUpdate()
    {
        CheckVolume();
        CheckState();
        IfSongEnded();
    }

    private void CheckVolume()
    {
        if (music01.volume != MusicVolume || music02.volume != MusicVolume)
        {
            music01.volume = MusicVolume;
            music02.volume = MusicVolume;
            audioService.InvokeMusicVolumeChanged(MusicVolume);
        }
    }

    private void CheckState()
    {
        if (audioService.GetCurrentState() != State)
        {
            audioService.InvokeAudioStateChanged(State);
            SwapSong();
        }
    }

    private void IfSongEnded()
    {
        if (!music01.isPlaying && !music02.isPlaying)
        {
            SwapSong();
        }
    }
    #endregion

    #region EventListners
    private void AudioService_OnSongChanged(object sender, Song song)
    {
        activeSong = song;
    }

    private void Audio_OnMusicVolumeChanged(object sender, float volume)
    {
        MusicVolume = volume;
    }

    private void Audio_OnAudioStateChanged(object sender, AudioState state)
    {
        State = state;
        SwapSong();
    }

    private void Level_OnLevelLoaded(int scene, Assets.Scripts.Room.Level level)
    {
        var newPlaylist = Playlists.GetByName(level?.SceneName);

        if (newPlaylist is null)
        {
            newPlaylist = Playlists.GetByName(level?.Gamemodes.FirstOrDefault()?.Name);
        }

        SetActivePlaylist(newPlaylist);
    }
    #endregion

    #region PrivateMethods
    private void SetActivePlaylist(Playlist playlist)
    {
        if (playlist is null)
        {
            playlist = Playlists.GetByName(null);
        }

        if (
            !(playlist is null) && (activePlaylist is null || !activePlaylist.name.Equals(playlist.name)))
        {
            activePlaylist = playlist;
            SwapSong();
        }
    }

    private Song GetRandomSongByState()
    {
        var songs = activePlaylist.songs.GetByType(audioService.GetCurrentState());
        List<Song> songsNotCurrent = null;
        var rnd = 0;

        if (!(songs is null) && !(activeSong is null))
        {
            songsNotCurrent = songs.Where(s => !s.Name.Equals(activeSong.Name)).ToList();
            rnd = UnityEngine.Random.Range(0, songsNotCurrent.Count);
        }
        
        if (!(activeSong is null) && !(songsNotCurrent is null) && songsNotCurrent.Count > 0)
        {
            return songsNotCurrent[rnd];
        }
        else
        {
            return songs?.First();
        }
    }

    private void SwapSong()
    {
        var song = GetRandomSongByState();

        if (song is null)
        {
            return;
        }

        var songIsSameAsActive = !(activeSong is null) && song.name.Equals(activeSong.name) && song.Type.Equals(audioService.GetCurrentState());

        //change songs if the state changes or none of the audiosources are playing
        if (!songIsSameAsActive || (!music01.isPlaying && !music02.isPlaying))
        {
            StopAllCoroutines();

            StartCoroutine(FadeBetweenStates(song));

            audioService.InvokeSongChanged(song);
        }
    }

    private IEnumerator SwapAudioSource(AudioSource from, AudioSource to, Song song)
    {
        to.clip = song.Clip;
        to.loop = AudioState.MainMenu.Equals(audioService.GetCurrentState());
        to.Play();
        yield return FadeVolume(from, to);
        from.Stop();
    }

    private IEnumerator FadeVolume(AudioSource from, AudioSource to)
    {
        float timeElapsed = 0;
        float timeToFade = 3;

        while (timeElapsed < timeToFade)
        {
            var function = timeElapsed / timeToFade;
            from.volume = Mathf.Lerp(MusicVolume, 0, function);
            to.volume = Mathf.Lerp(0, MusicVolume, function);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator FadeBetweenStates(Song song)
    {
        if (music01.isPlaying)
        {
            yield return SwapAudioSource(music01, music02, song);
        }
        else
        {
            yield return SwapAudioSource(music02, music01, song);
        }
    }
    #endregion
}

public enum AudioState
{
    MainMenu,
    Combat,
    Neutral,
    Ambient,
    Action
}
