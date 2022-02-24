using Assets.Scripts.Events.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Events
{
    public delegate void OnSongChanged(SongChangedEvent songChangedEvent);
    public delegate void OnPlaylistChanged(PlaylistChangedEvent playlistChangedEvent);
}
