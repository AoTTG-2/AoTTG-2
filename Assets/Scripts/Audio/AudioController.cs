using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using System.Threading.Tasks;

[System.Serializable]
public class PlaylistPack
{
    public string sceneName;

    public Playlist menuPlaylist;
    public Playlist combatPlaylist;
    public Playlist neutralPlaylist;
    public Playlist ambientPlaylist;
}

public class AudioController : MonoBehaviour
{
    private float volume;
    public ChannelTypes CurrentState
    {
        get { return jukebox.CurrentState; }
    }
    public event EventHandler<float> OnVolumeChange;

    private Jukebox jukebox;

    public static AudioController Instance { get; private set; }

    public Channel[] channels = new Channel[4]; //Channels for: MainMenu, Combat, Neutral, Ambient

    public List<PlaylistPack> scenePlaylists = new List<PlaylistPack>();

    public event EventHandler<ChannelTypes> OnStateChange;

    protected void Awake()
    {
        CheckSingleton(); //Delete itself if AudioController exists
        InitJukeBox();
        SetVolume(0.5f);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5)) SetState(ChannelTypes.MainMenu); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha6)) SetState(ChannelTypes.Combat); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha7)) SetState(ChannelTypes.Neutral); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha8)) SetState(ChannelTypes.Ambient); //DEBUGGING, DELETE IT
    }

    protected void FixedUpdate()
    {
        jukebox.CheckSongEnded();
    }

    public void SetState(ChannelTypes type)
    {
        OnStateChange.Invoke(this, type);
    }

    private void CheckSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log(gameObject);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var playlist = scenePlaylists.First(sp => sp.sceneName.Equals(scene.name));
        LoadPlaylistPack(playlist);
    }

    private void InitJukeBox()
    {
        if (jukebox is null)
        {
            jukebox = new Jukebox(channels);
        }

        jukebox.Start();
    }

    private void LoadPlaylistPack(PlaylistPack pack)
    {
        if (pack.menuPlaylist == null || pack.combatPlaylist == null || pack.neutralPlaylist == null || pack.ambientPlaylist == null)
        {
            Debug.LogWarning("Missing Playlists in scenePlaylistPack");
            channels[0].playlist = scenePlaylists[0].menuPlaylist;
            channels[1].playlist = scenePlaylists[0].combatPlaylist;
            channels[2].playlist = scenePlaylists[0].neutralPlaylist;
            channels[3].playlist = scenePlaylists[0].ambientPlaylist;
        }
        else
        {
            channels[0].playlist = pack.menuPlaylist;
            channels[1].playlist = pack.combatPlaylist;
            channels[2].playlist = pack.neutralPlaylist;
            channels[3].playlist = pack.ambientPlaylist;
        }

        jukebox.SetChannels(channels);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
        OnVolumeChange.Invoke(this, volume);
    }
}

public enum ChannelTypes
{
    MainMenu,
    Combat,
    Neutral,
    Ambient
}
