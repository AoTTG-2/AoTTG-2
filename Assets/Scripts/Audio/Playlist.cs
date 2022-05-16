using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    [CreateAssetMenu(fileName = "New Playlist", menuName = "Music/Playlist")]
    public class Playlist : ScriptableObject
    {
        public List<Song> songs;
    }
}
