using YoutubeExplode.Playlists;

namespace YouTubeBackup.Models
{
    public class FullPlaylist
    {
        public Playlist BasePlaylist { get; private set; }
        public IEnumerable<PlaylistVideo> Videos { get; private set; }

        public string Title { get; private set; }

        public FullPlaylist(Playlist basePlaylist, IEnumerable<PlaylistVideo> videos, string title = null)
        {
            BasePlaylist = basePlaylist;
            Videos = videos;
            Title = basePlaylist?.Title ?? title;
        }
    }
}
