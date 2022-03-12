using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public static class AudioExtensions
    {
        /// <summary>
        /// Gets playlist with name <paramref name="name"/>
        /// </summary>
        /// <param name="list"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Playlist GetByName(this List<Playlist> list, string name)
        {
            var playlist = list.FirstOrDefault(p => p.name.Equals(name));
            return playlist;
        }

        /// <summary>
        /// Returns the playlist named Default
        /// </summary>
        /// <param name="playlists"></param>
        /// <returns></returns>
        public static Playlist GetDefault(this List<Playlist> playlists)
        {
            return playlists.FirstOrDefault(p => p.name.Equals("Default", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all <see cref="Song"/> of a certain <see cref="MusicState"/>.
        /// </summary>
        /// <param name="state"><see cref="MusicState"/></param>
        public static List<Song> GetByState(this List<Song> songs, MusicState state)
        {
            return songs.Where(s => s.Type.Equals(state)).ToList();
        }

        /// <summary>
        /// Gets a random <see cref="Song"/> by <paramref name="state"/>.
        /// </summary>
        /// <param name="state"><see cref="MusicState"/></param>
        public static Song GetRandomByState(this List<Song> songs, MusicState state)
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

        /// <summary>
        /// Converts <see cref="float"/> to <see cref="AudioMixer"/> volume;
        /// </summary>
        /// <param name="volume">Must be a value between 0,0 and 1.0 for a correct conversion</param>
        /// <returns>Mathf.Log10(<paramref name="volume"/>) * 20</returns>
        public static float ToLogVolume(this float volume)
        {
            return Mathf.Log10(volume) * 20;
        }
    }
}
