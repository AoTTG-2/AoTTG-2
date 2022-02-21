using Assets.Scripts.Audio;
using Assets.Scripts.Base;
using Assets.Scripts.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : SingeltonMonoBehaviour<AudioController>
{
    #region Events
    public event EventHandler<AudioState> OnStateChanged;

    protected virtual void RaiseStateChanged(AudioState state)
    {
        EventHandler<AudioState> handler = OnStateChanged;
        handler?.Invoke(this, state);
    }
    #endregion

    #region PrivateProperties
    private AudioState activeState;
    private Playlist activePlaylist;
    private Song activeSong;
    #endregion

    #region PublicProperties
    public AudioSource music01, music02;
    public string NowPlaying => activeSong.Name;
    public List<Playlist> Playlists;
    public float MusicVolume = 0.5f;
    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Service.Level.OnLevelLoaded += Level_OnLevelLoaded;
        SetActivePlaylist(null);
    }

    private void Level_OnLevelLoaded(int scene, Assets.Scripts.Room.Level level)
    {
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var newPlaylist = Playlists.GetByName(scene.name);
        SetActivePlaylist(newPlaylist);
    }
    #endregion

    #region PublicMethods
    public void SetState(AudioState state)
    {
        Debug.Log($"to state {state} from state {activeState}");
        activeState = state;
        SwapSong();
        RaiseStateChanged(state);
    }

    public AudioState GetActiveState()
    {
        return activeState;
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

    private Song GetSongByState()
    {
        return activePlaylist.songs.GetByType(activeState);
    }

    private void SwapSong()
    {
        var song = GetSongByState();

        if (song is null)
        {
            return;
        }

        var songIsSameAsActive = !(activeSong is null) && song.name.Equals(activeSong.name) && song.Type.Equals(activeState);

        if (!songIsSameAsActive)
        {
            StopAllCoroutines();

            StartCoroutine(FadeBetweenSongs(song));

            activeSong = song;
        }
    }

    private IEnumerator SwapAudioSource(AudioSource from, AudioSource to, Song song)
    {
        to.clip = song.Clip;
        to.loop = true;
        to.volume = 0.5f;
        to.Play();
        yield return FadeVolume(from, to);
        from.Stop();
    }

    private IEnumerator FadeVolume(AudioSource from, AudioSource to)
    {
        float timeElapsed = 0;
        float timeToFade = 3f;

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
    private IEnumerator FadeBetweenSongs(Song song)
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
    Ambient
}
