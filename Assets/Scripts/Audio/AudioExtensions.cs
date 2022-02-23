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

        public static List<Song> GetByState(this List<Song> songs, AudioState? state)
        {
            return songs.Where(s => s.Type.Equals(state)).ToList();
        }

        public static List<Song> GetByName(this List<Song> songs, string name)
        {
            return songs.Where(s => s.Name.Equals(name)).ToList();
        }

        public static Song GetRandomByState(this List<Song> songs, AudioState? state)
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

        public static bool Equals(this AudioMixerSnapshot snapshot, AudioState state)
        {
            return snapshot.name.Equals(state.ToString());
        }
    }
}
