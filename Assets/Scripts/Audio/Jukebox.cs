using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Audio;

public class Jukebox
{
    private bool isSwitching = false;
    private List<Channel> channels;
    private Channel currentChannel;

    public ChannelTypes CurrentState 
    {
        get { return currentChannel.channelType; }   
    }
    public string CurrentSongName
    {
        get { return currentChannel.CurrentSong?.songName; }
    }

    public float Volume 
    {
        get { return currentChannel.Volume; }
    }

    public Jukebox(Channel[] channels)
    {
        this.channels = channels.ToList();
        SetCurrentChannel(GetChannel(ChannelTypes.MainMenu));
        AudioController.Instance.OnStateChange += SwitchChannel;
        AudioController.Instance.OnVolumeChange += ChangeVolume;
    }

    private void ChangeVolume(object sender, float volume)
    {
        currentChannel.Volume = volume;
    }

    private void SetCurrentChannel(Channel channel)
    {
        currentChannel = channel;
    }

    public void Start()
    {
        channels.ForEach(c => c.StartPlaying());
    }

    public void CheckSongEnded()
    {
        channels.Where(c => !c.IsPlaying).ToList().ForEach(c => c.PlayNextSong());
    }

    public Channel GetChannel(ChannelTypes type)
    {
        return channels.FirstOrDefault(c => c.channelType.Equals(type));
    }

    private void SwitchChannel(object sender, ChannelTypes type)
    {
        if (currentChannel.channelType != type && !isSwitching)
        {
            isSwitching = true;
            var oldChannel = GetChannel(currentChannel.channelType);
            var transitiontime = 1f;
            Debug.Log($"from: {currentChannel.channelType}");
            Debug.Log($"to: {type}");
            isSwitching = true;
            SetCurrentChannel(GetChannel(type));
            currentChannel.snapshot.TransitionTo(transitiontime);
            //oldChannel.mixerGroup.audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[2] { oldChannel.snapshot, currentChannel.snapshot }, new float[2] { .2f, .80f }, transitiontime);
            isSwitching = false;
        }
    }

    internal void SetChannels(Channel[] channels)
    {
        this.channels = channels.ToList();
    }
}
