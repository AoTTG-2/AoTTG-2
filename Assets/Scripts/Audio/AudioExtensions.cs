using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Audio
{
    public static class AudioExtensions
    {
        public static Playlist GetByName(this List<Playlist> list, string name)
        {
            var playlist = list.FirstOrDefault(p => p.name.Equals(name));
            return playlist;
        }

        public static Playlist GetDefault(this List<Playlist> list)
        {
            return list.FirstOrDefault(p => p.name.Equals("Default", StringComparison.OrdinalIgnoreCase));
        }

        public static List<Song> GetByState(this List<Song> songs, MusicState? state)
        {
            return songs.Where(s => s.Type.Equals(state)).ToList();
        }

        public static Song GetRandomByState(this List<Song> songs, MusicState? state)
        {
            var filteredSongs = songs.Where(s => s.Type.Equals(state)).ToList();
            var count = filteredSongs.Count;
            if (count > 1)
            {
                var rnd = UnityEngine.Random.Range(0, count);
                return filteredSongs[rnd];
            }
            else
            {
                return filteredSongs.FirstOrDefault();
            }
            
        }
    }
}
