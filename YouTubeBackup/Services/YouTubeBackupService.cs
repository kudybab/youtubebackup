using YouTubeBackup.Globals;
using YouTubeBackup.Helpers;
using YouTubeBackup.Models;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;

namespace YouTubeBackup.Services
{

    public interface IYouTubeBackupService
    {
        Task Backup();
    }

    public class YouTubeBackupService : IYouTubeBackupService
    {
        private readonly YoutubeClient _client;

        public YouTubeBackupService()
        {
            _client = new YoutubeClient();
        }

        public async Task Backup()
        {

            var playlists = new List<string>
            {
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSld-dlJy64CZDODD2pOSSMV", // ToDo
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSnLldVA5aI7qj_OWF-7KYY9",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSk_1C_GAIt_kvFQpewLNZPe",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSkItk2R0XK9RcN83zi-YLNm",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSm1s6ANwTtOEwjnjX8ALAmS", // ok v2
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSlPodptBP5QOqch-DWf0DDH",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSm7tOyLtPZgBhYCj-GoVEWa",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSmSaj3creKJOr4AjKbZ72z4",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSmshzaKomtMdkpE6NytA0hi",
                //"https://www.youtube.com/playlist?list=PLai1TNsgguSng6rJi_z18vzprESdJuiI_",
                //"https://www.youtube.com/watch?list=PLai1TNsgguSmK01X0il_bxVedWT8EDk-6", // x
            };

            foreach (var playlistUrl in playlists)
            {
                Console.WriteLine($"Playlist {playlists.IndexOf(playlistUrl) +1} out of {playlists.Count()}");
                await DownloadPlaylist(playlistUrl);
            }

            Console.WriteLine("Wanna check for duplicates? (y/n)");
            var userInput = Console.ReadLine()?.Trim().ToLower();
            if (userInput == "y")
            {
                Console.WriteLine();
                Console.WriteLine("Checking for duplicates...");
                new DuplicatesCheckService().CheckForDuplicates();
            }
        }

        private async Task DownloadPlaylist(string playlistUrl)
        {
            YoutubeHelpers.TryParsePlaylistId(playlistUrl, out var playlistId);
            var playlist = await _client.Playlists.GetAsync(playlistId);
            var list = new FullPlaylist(playlist, await _client.Playlists.GetVideosAsync(playlist.Id).CollectAsync());
            if (!Directory.Exists(@$"{GlobalConfig.SONGS_DIRECTORY}\{list.Title}"))
                Directory.CreateDirectory(@$"{GlobalConfig.SONGS_DIRECTORY}\{list.Title}");

            var outputPath = @$"{GlobalConfig.SONGS_DIRECTORY}\{list.Title}";
            foreach (var video in list.Videos)
            {
                var videoName = PathSanitizer.SanitizeFilename(video.Title, ' ');
                var outputFilePath = @$"{outputPath}\{videoName}.mp3";

                if (File.Exists(outputFilePath))
                {
                    continue;
                }
                try
                {
                    Console.WriteLine($"Downloading {videoName}");
                    await _client.Videos.DownloadAsync(video.Id, outputFilePath);
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
