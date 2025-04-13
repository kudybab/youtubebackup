using YouTubeBackup.Globals;

namespace YouTubeBackup.Helpers
{
    public class LocalSongsStorage
    {
        public IEnumerable<string> GetLocalStorageTitles()
        {
            var allFiles = new List<string>().AsEnumerable();
            var directories = Directory.GetDirectories(GlobalConfig.SONGS_DIRECTORY);
            foreach (var directory in directories)
            {
                allFiles = allFiles.Concat(Directory.GetFiles(directory));
            }

            return allFiles.Select(x => x.Split('\\').Last());
        }
    }
}
