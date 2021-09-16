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
        PlaySong("neutral", 0);
        PlaySong("combat", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (HasSongEnded(neutralAudioSource))
        {
            PlayNextSong("neutral");
        }
        if (HasSongEnded(combatAudioSource))
        {
            PlayNextSong("combat");
        }
    }

    public void PlayNextSong(string type)
    {
        PlaySong(type, (type == "neutral" ? neutralPlaylistIndex : combatPlaylistIndex) + 1);
    }

    public void SwitchMusic(AudioMixerSnapshot snapshot)
    {
        currentSnapshot = snapshot;
        currentSnapshot.TransitionTo(transitionTime);
    }

    private bool HasSongEnded(AudioSource audioS)
    {
        return (!audioS.isPlaying);
    }

    public void NeutralCombatSwap()
    {
        SwitchMusic(currentSnapshot == neutralSnapshot ? combatSnapshot : neutralSnapshot);
    }

    public void LoadPlaylist(string type, Playlist playlist)
    {
        if (type == "neutral")
        {
            neutralPlaylist = playlist;
            neutralPlaylistIndex = 0;
            combatPlaylistIndex = 0;
        }
        else if (type == "combat")
        {
            combatPlaylist = playlist;
            neutralPlaylistIndex = 0;
            combatPlaylistIndex = 0;
        }
        PlaySong("neutral", 0);
        PlaySong("combat", 0);
    }

    public void PlaySong(string type, int index) {
        if (type == "neutral")
        {
            neutralPlaylistIndex = index;
            neutralPlaylistIndex = neutralPlaylistIndex % neutralPlaylist.songs.Length;
        } else if (type == "combat")
        {
            combatPlaylistIndex = index;
            combatPlaylistIndex = combatPlaylistIndex % combatPlaylist.songs.Length;
        }

        LoadSongSettings(type);
        (type == "neutral" ? neutralAudioSource : combatAudioSource).Play();
    }

    private void LoadSongSettings(string type)
    {
        Song currentSong;
        switch (type)
        {
            case "neutral":
                currentSong = neutralPlaylist.songs[neutralPlaylistIndex];
                neutralAudioSource.volume = currentSong.volume;
                neutralAudioSource.clip = currentSong.clip;
                break;
            case "combat":
                currentSong = combatPlaylist.songs[combatPlaylistIndex];
                combatAudioSource.volume = currentSong.volume;
                combatAudioSource.clip = currentSong.clip;
                break;
        }
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
