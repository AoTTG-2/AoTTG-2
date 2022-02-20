using UnityEngine.Audio;
using UnityEngine;
using System.Linq;

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
    public ChannelTypes channelType;
    public Song CurrentSong;

    public float Volume
    {
        get { return audioSource.volume; }
        set { audioSource.volume = value; }
    }

    public bool IsPlaying
    {
        get { return audioSource.isPlaying; }
    }


    public void StartPlaying()
    {
        ChangeSong(0);
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Play(int index)
    {
        ChangeSong(index);
        audioSource.Play();
    }

    public void PlayNextSong()
    {
        var nextIndex = playlist.songs.ToList().IndexOf(CurrentSong) + 1;
        ChangeSong(nextIndex);
    }

    private void ChangeSong(int index)
    {
        index = (index <= playlist.songs.Length - 1) ? index : 0;

        CurrentSong = playlist.songs[index];
        audioSource.clip = CurrentSong.clip;
        audioSource.Play();
        Debug.Log($"Now playing: {CurrentSong.songName} on channel {channelType}");
    }
}
