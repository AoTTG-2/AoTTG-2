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
            return playlist is null ? list.FirstOrDefault(p => p.name.Equals("Default", StringComparison.OrdinalIgnoreCase)) : playlist;
        }

        public static Song GetByType(this List<Song> songs, AudioState state)
        {
            return songs.FirstOrDefault(s => s.Type.Equals(state));
        }
    }
}
