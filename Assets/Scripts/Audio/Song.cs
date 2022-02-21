using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Song", menuName = "Song")]
public class Song : ScriptableObject
{
    public string Name { get { return Clip.name; } }
    public AudioClip Clip;
    public AudioState Type;
}
