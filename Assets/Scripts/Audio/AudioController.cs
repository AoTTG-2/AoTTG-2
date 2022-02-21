using Assets.Scripts.Audio;
using Assets.Scripts.Base;
using System;
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
    #endregion

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SetActivePlaylist(null);
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
        activeState = state;
        var newSong = GetSongByState();
        SwapSong(newSong);
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
            (activePlaylist is null || !activePlaylist.name.Equals(playlist.name))
            && !(playlist is null))
        {
            activePlaylist = playlist;
            var newSong = GetSongByState();
            SwapSong(newSong);
        }
    }

    private Song GetSongByState()
    {
        var song = activePlaylist.songs.GetByType(activeState);
        return song is null ? activePlaylist.songs.First() : song;
    }

    private void SwapSong(Song song)
    {
        SwapAudioSource(song);
        Debug.Log(activePlaylist.name);
    }

    private void SwapAudioSource(Song song)
    {
        if (music01.isPlaying)
        {
            music02.clip = song.Clip;
            music02.loop = true;
            music02.volume = 0.5f;
            music02.Play();
            music01.Stop();
        }
        else
        {
            music01.clip = song.Clip;
            music01.loop = true;
            music01.volume = 0.5f;
            music01.Play();
            music02.Stop();
        }

        activeSong = song;
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
