using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Channel
{
    public int index;
    public AudioMixerGroup mixerGroup;
    public AudioMixerSnapshot snapshot;
    public Playlist playlist;
    public int playlistIndex;
    public AudioSource audioSource;
    public Coroutine fadeInCo = null;
}

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public Channel[] channels = new Channel[4];
    private Channel currentChannel;

    public float transitionTime = 1.5f; //Duration of the transition between neutral and combat snapshots

    void Awake()
    {
        CheckSingleton();
        currentChannel = GetChannel(ChannelTypes.MainMenu);
    }

    private void Start()
    {
        PlayRandomSong(channels[0]);
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

    public void LoadPlaylist(Channel channel, Playlist playlist) //Load different playlist, mostly used when changing scenes with own playlists
    {
        channel.playlist = playlist;
        channel.playlistIndex = 0;
        PlaySong(channel, channel.playlistIndex);

        SwitchMusic(channel);
    }

    public void SwitchMusic(Channel channel) //Select snapshot to transition between neutral and combat
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

    private Channel GetChannel(ChannelTypes type)
    {
        return channels[(int) type];
    }
}

enum ChannelTypes
{
    MainMenu,
    Combat,
    Neutral,
    Ambient
}
