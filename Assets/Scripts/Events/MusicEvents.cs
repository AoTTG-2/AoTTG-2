using Assets.Scripts.Events.Args;

namespace Assets.Scripts.Events
{
    public delegate void OnSongChanged(SongChangedEvent songChangedEvent);
    public delegate void OnPlaylistChanged(PlaylistChangedEvent playlistChangedEvent);
}
