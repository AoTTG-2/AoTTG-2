using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    public float transitionTime = 1.5f; //Duration of the transition between neutral and combat snapshots

    public AudioMixerGroup neutralMixerGroup;
    public AudioMixerGroup combatMixerGroup;
    public AudioMixerSnapshot neutralSnapshot;
    public AudioMixerSnapshot combatSnapshot;
    private AudioMixerSnapshot currentSnapshot;

    public Playlist neutralPlaylist;
    public Playlist combatPlaylist;
    private int neutralPlaylistIndex = 0;
    private int combatPlaylistIndex = 0;
    public AudioSource neutralAudioSource;
    public AudioSource combatAudioSource;
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
        if (HasSongEnded(neutralAudioSource)) //Loops the songs in the playlists
        {
            PlayNextSong("neutral");
        }
        if (HasSongEnded(combatAudioSource))
        {
            PlayNextSong("combat");
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) NeutralCombatSwap(); //DEBUGGING, DELETE IT
        if (Input.GetKeyDown(KeyCode.Alpha2)) PlayNextSong("neutral"); //DEBUGGING, DELETE IT
    }

    public void PlayNextSong(string type) //Play next song in playlist where type is neutral or combat playlist
    {
        PlaySong(type, (type == "neutral" ? neutralPlaylistIndex : combatPlaylistIndex) + 1);
    }

    public void SwitchMusic(AudioMixerSnapshot snapshot) //Select snapshot to transition between neutral and combat
    {
        currentSnapshot = snapshot;
        currentSnapshot.TransitionTo(transitionTime);
    }

    private bool HasSongEnded(AudioSource audioS)
    {
        return (!audioS.isPlaying);
    }

    public void NeutralCombatSwap() //Uses opposite snapshot of currently playing
    {
        SwitchMusic(currentSnapshot == neutralSnapshot ? combatSnapshot : neutralSnapshot);
    }

    public void LoadPlaylist(string type, Playlist playlist) //Load different playlist, mostly used when changing scenes with own playlists
    {
        if (type == "neutral") //Play the playlist from the beginning
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
        currentSnapshot = neutralSnapshot;
        SwitchMusic(currentSnapshot);
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

    private void LoadSongSettings(string type) //Load volume, clip etc. for current song
    {
        Song currentSong;
        switch (type)
        {
            case "neutral":
                currentSong = neutralPlaylist.songs[neutralPlaylistIndex];
                neutralAudioSource.volume = 0f;
                StartCoroutine(FadeIn(neutralAudioSource, currentSong.volume)); //Fade in the volume of song
                neutralAudioSource.clip = currentSong.clip;
                break;
            case "combat":
                currentSong = combatPlaylist.songs[combatPlaylistIndex];
                combatAudioSource.volume = 0f;
                StartCoroutine(FadeIn(combatAudioSource, currentSong.volume));
                combatAudioSource.clip = currentSong.clip;
                break;
        }
    }

    IEnumerator FadeIn(AudioSource audioSource, float target, float time = 2f)
    {
        while(audioSource.volume < target)
        {
            audioSource.volume += (Time.deltaTime / time) * target;
            yield return null;
        }
        audioSource.volume = target;
        yield break;
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
