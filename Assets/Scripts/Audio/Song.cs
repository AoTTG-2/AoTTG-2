using UnityEngine;

namespace Assets.Scripts.Audio
{
    [CreateAssetMenu(fileName = "New Song", menuName = "Music/Song")]
    public class Song : ScriptableObject
    {
        public string Name => Clip.name;
        public string Composer;
        public AudioClip Clip;
        [Tooltip("Decides in what state to trigger this song")]
        public MusicState Type;
    }
}