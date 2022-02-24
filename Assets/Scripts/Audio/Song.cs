using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class Song : ScriptableObject
{
    public string Name { get { return Clip.name; } }
    public string Composer;
    public AudioClip Clip;
    [Tooltip("Decides in what state to trigger this song")]
    public MusicState Type;
}
