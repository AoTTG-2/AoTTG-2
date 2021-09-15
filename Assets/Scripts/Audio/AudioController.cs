using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public AudioMixerGroup neutralMixerGroup;
    public AudioMixerGroup combatMixerGroup;

    public Playlist neutralPlaylist;
    public Playlist combatPlaylist;
    public AudioSource neutralAudioSource;
    public AudioSource combatAudioSource;
    void Awake()
    {
        CheckSingleton();
        LoadAudioSources();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadAudioSources()
    {
        neutralAudioSource = gameObject.AddComponent<AudioSource>();
        neutralAudioSource.playOnAwake = false;
        neutralAudioSource.outputAudioMixerGroup = neutralMixerGroup;

        combatAudioSource = gameObject.AddComponent<AudioSource>();
        combatAudioSource.playOnAwake = false;
        combatAudioSource.outputAudioMixerGroup = combatMixerGroup;
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
}
