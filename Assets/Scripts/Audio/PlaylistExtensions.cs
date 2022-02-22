using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Audio
{
    public static class PlaylistExtensions
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

        public static List<Song> GetByType(this List<Song> songs, AudioState state)
        {
            return songs.Where(s => s.Type.Equals(state)).ToList();
        }

        public static List<Song> GetByName(this List<Song> songs, string name)
        {
            return songs.Where(s => s.Name.Equals(name)).ToList();
        }
    }
}
