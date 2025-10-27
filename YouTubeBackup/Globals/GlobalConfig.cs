namespace YouTubeBackup.Globals
{
    public class GlobalConfig
    {
        public static string WORKING_DIRECTORY = @"C:\Users\local\Desktop\Youtube";
        public static string SONGS_DIRECTORY = @"C:\Users\local\Desktop\Youtube\Songs";

        public static double DUPLICATE_CHECK_BASE_THRESHOLD = 0.65;
        public static double DUPLICATE_CHECK_BASE_THRESHOLD_SHORT_TITLES_BELOW_20_CHARACTERS = 0.4;
    }
}
