namespace YouTubeBackup.Helpers
{
    public class FileHelpers
    {
        public void CreateFileIfNotExists(string fullFilePath)
        {
            if (!File.Exists(fullFilePath))
            {
                string directoryPath = Path.GetDirectoryName(fullFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(fullFilePath, "{}"); 
            }
        }
    }
}
