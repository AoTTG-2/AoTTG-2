using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Playlist", menuName = "Playlist")]
public class Playlist : ScriptableObject
{
    public string playlistName;
    public Song[] songs;

    public string type = "neutral";
}
