using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class Song : ScriptableObject
{
    public string songName;
    public AudioClip clip;

    public string type = "neutral";

    [Range(0f, 1f)]
    public float volume = 0.5f;
}
