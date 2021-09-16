using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Channel //Each channel holds it's own references, thanks to that every method can just take in channel as a parameter and not worry which one it is
{
    public int index; //Remembers it's own index in the channels array INDEX HAS TO MATCH CHANNELTYPE ORDER
    public AudioMixerGroup mixerGroup;
    public AudioMixerSnapshot snapshot;
    public Playlist playlist;
    public int playlistIndex;
    public AudioSource audioSource;
    public Coroutine fadeInCo = null; //Made so coroutine can be stopped if song is changed during it
}

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
    public static AudioController Instance { get; private set; }

    public Channel[] channels = new Channel[4]; //Channels for: MainMenu, Combat, Neutral, Ambient
    private Channel currentChannel;

    public float transitionTime = 1.5f; //Duration of the transition between snapshots

    private Coroutine checkStateCo;

    public bool[] stateLayers = { true, false, false, true };


    public PlaylistPack[] scenePlaylists = new PlaylistPack[0];

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckSingleton(); //Delete itself if AudioController exists
        currentChannel = GetChannel(ChannelTypes.MainMenu); //Set default current channel to main menu
    }

    private void Start()
    {
        checkStateCo = StartCoroutine(CheckState(0.5f));

        PlayRandomSong(channels[0]); //Start song for every channel
        PlayRandomSong(channels[1]);
        PlayRandomSong(channels[2]);
        PlayRandomSong(channels[3]);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < channels.Length; i++)
        {
            if (HasSongEnded(channels[i])) //Loops the songs in the playlists
            {
                PlayNextSong(channels[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchMusic(channels[(currentChannel.index + 1) % channels.Length]); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlayRandomSong(currentChannel); //DEBUGGING, DELETE IT

        if (Input.GetKeyDown(KeyCode.Alpha5)) SwapState(ChannelTypes.MainMenu); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha6)) SwapState(ChannelTypes.Combat); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha7)) SwapState(ChannelTypes.Neutral); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha8)) SwapState(ChannelTypes.Ambient); //DEBUGGING, DELETE IT
    }

    public void PlayNextSong(Channel channel) //Play next song in playlist where type is neutral or combat playlist
    {
        PlaySong(channel, channel.playlistIndex + 1);
    }

    public void PlayRandomSong(Channel channel) //Play random song in playlist where type is neutral or combat playlist
    {
        int randomInt = Random.Range(0, channel.playlist.songs.Length);
        if (randomInt == channel.playlistIndex) randomInt++;
        PlaySong(channel, randomInt);
    }

    private bool HasSongEnded(Channel channel)
    {
        return (!channel.audioSource.isPlaying);
    }

    private void SwitchMusic(Channel channel) //Select snapshot to transition between neutral and combat
    {
        currentChannel = channel;
        channel.snapshot.TransitionTo(transitionTime);
    }

    public void PlaySong(Channel channel, int index) {
        channel.playlistIndex = index;
        channel.playlistIndex = channel.playlistIndex % channel.playlist.songs.Length;

        LoadSongSettings(channel);
        channel.audioSource.Play();
    }

    private void LoadSongSettings(Channel channel) //Load volume, clip etc. for current song
    {
        Song currentSong;
        currentSong = channel.playlist.songs[channel.playlistIndex];
        channel.audioSource.volume = 0f;
        if (channel.fadeInCo != null) StopCoroutine(channel.fadeInCo);
        channel.fadeInCo = StartCoroutine(FadeIn(channel.audioSource, currentSong.volume));
        channel.audioSource.clip = currentSong.clip;
    }

    IEnumerator FadeIn(AudioSource audioSource, float target, float time = 2f)
    {
        float progress = 0f;
        while(progress < 1f)
        {
            progress += (Time.deltaTime / time);
            audioSource.volume = (progress * progress) * target;

            yield return null;
        }
        audioSource.volume = target;
        yield break;
    }

    IEnumerator CheckState(float interval)
    {
        int stateIndex;

        while (true)
        {
            stateIndex = GetStateIndex();
            if (currentChannel.index != stateIndex)
            {
                SwitchMusic(channels[stateIndex]);
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private int GetStateIndex()
    {
        for(int i = 0; i < stateLayers.Length; i++)
        {
            if (stateLayers[i]) return i;
        }
        return stateLayers.Length-1;
    }

    public void SetState(ChannelTypes type, bool value)
    {
        stateLayers[(int) type] = value;
    }

    public void SwapState(ChannelTypes type)
    {
        bool currentState = stateLayers[(int) type];
        SetState(type, !currentState);
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
        for (int i = 0; i < scenePlaylists.Length; i++)
        {
            if (scene.name == scenePlaylists[i].sceneName)
            {
                LoadPlaylistPack(scenePlaylists[i]);
                stateLayers[0] = false;
                stateLayers[1] = false;
                stateLayers[2] = false;
                stateLayers[3] = true;

                if (scene.name == "AoTTG 2") stateLayers[0] = true;

                return;
            }
        }
        LoadPlaylistPack(scenePlaylists[0]);
        stateLayers[0] = false;
        stateLayers[1] = false;
        stateLayers[2] = false;
        stateLayers[3] = true;
    }

    private void LoadPlaylistPack(PlaylistPack pack)
    {
        if(pack.menuPlaylist == null || pack.combatPlaylist == null || pack.neutralPlaylist == null || pack.ambientPlaylist == null)
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

        PlayRandomSong(channels[0]);
        PlayRandomSong(channels[1]);
        PlayRandomSong(channels[2]);
        PlayRandomSong(channels[3]);
    }

    private Channel GetChannel(ChannelTypes type)
    {
        return channels[(int) type];
    }
}

public enum ChannelTypes
{
    MainMenu,
    Combat,
    Neutral,
    Ambient
}
