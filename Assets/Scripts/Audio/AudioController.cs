using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioMixerGroup neutralMixerGroup;
    public AudioMixerGroup combatMixerGroup;

    public Playlist neutralPlaylist;
    public Playlist combatPlaylist;
    public AudioSource neutralAudioSource;
    public AudioSource combatAudioSource;
    void Awake()
    {
        neutralAudioSource = gameObject.AddComponent<AudioSource>();
        neutralAudioSource.playOnAwake = false;
        neutralAudioSource.outputAudioMixerGroup = neutralMixerGroup;

        combatAudioSource = gameObject.AddComponent<AudioSource>();
        combatAudioSource.playOnAwake = false;
        combatAudioSource.outputAudioMixerGroup = combatMixerGroup;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
