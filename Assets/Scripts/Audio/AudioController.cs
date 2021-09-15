using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public float transitionTime = 1.5f;

    public AudioMixerGroup neutralMixerGroup;
    public AudioMixerGroup combatMixerGroup;
    public AudioMixerSnapshot neutralSnapshot;
    public AudioMixerSnapshot combatSnapshot;
    private AudioMixerSnapshot currentSnapshot;

    public Playlist neutralPlaylist;
    public Playlist combatPlaylist;
    private int neutralPlaylistIndex = 0;
    public AudioSource neutralAudioSource;
    public AudioSource combatAudioSource;
    private int combatPlaylistIndex = 0;
    void Awake()
    {
        CheckSingleton();
        LoadAudioSources();
        currentSnapshot = neutralSnapshot;
    }

    private void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchMusic(AudioMixerSnapshot snapshot)
    {
        currentSnapshot = snapshot;
        currentSnapshot.TransitionTo(transitionTime);
    }

    public void NeutralCombatSwap()
    {
        SwitchMusic(currentSnapshot == neutralSnapshot ? combatSnapshot : neutralSnapshot);
    }

    private void PlayMusic()
    {
        Song currentNeutralSong = neutralPlaylist.songs[neutralPlaylistIndex];
        Song currentCombatSong = combatPlaylist.songs[combatPlaylistIndex];

        neutralAudioSource.volume = currentNeutralSong.volume;
        combatAudioSource.volume = currentCombatSong.volume;

        neutralAudioSource.clip = currentNeutralSong.clip;
        combatAudioSource.clip = currentCombatSong.clip;

        neutralAudioSource.Play();
        combatAudioSource.Play();
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
