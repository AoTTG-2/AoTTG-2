using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public Playlist neutralPlaylist;
    public Playlist combatPlaylist;
    public AudioSource neutralAudioSource;
    public AudioSource combatAudioSource;
    void Awake()
    {
        neutralAudioSource = gameObject.AddComponent<AudioSource>();
        combatAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
