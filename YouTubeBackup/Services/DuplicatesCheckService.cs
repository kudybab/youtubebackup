using Newtonsoft.Json;
using YouTubeBackup.Extensions;
using YouTubeBackup.Globals;
using YouTubeBackup.Helpers;

namespace YouTubeBackup.Services
{
    internal class DuplicatesCheckService
    {
        private string _configFileName;
        private DuplicatesConfig _config;

        public DuplicatesCheckService()
        {
            _configFileName = GlobalConfig.WORKING_DIRECTORY + @"\config\duplicates.json";
            new FileHelpers().CreateFileIfNotExists(_configFileName);
            _config = JsonConvert.DeserializeObject<DuplicatesConfig>(File.ReadAllText(_configFileName));
        }

        public void CheckForDuplicates()
        {
            IEnumerable<string> localTitles = new LocalSongsStorage().GetLocalStorageTitles();
            List<List<string>> detectedDuplicates = new DuplicatesCheckCore().FindDuplicateSongs(localTitles.ToList());

            foreach (List<string> duplicatedTitles in detectedDuplicates)
            {
                DetectedDuplicate(duplicatedTitles);
            }
        }

        private void DetectedDuplicate(List<string> duplicatedTitles)
        {
            bool configRecordExists = _config.DuplicatedRecords.Any(x => x.Titles.ContainsAll(duplicatedTitles));
            DuplicatedRecord configRecord = null;
            try
            {
            configRecord = configRecordExists
                ? _config.DuplicatedRecords.Single(x => x.Titles.ContainsAll(duplicatedTitles))
                : null;

            }
            catch (Exception ex)
            {

            }

            if (configRecordExists)
            {
                if (configRecord.IsDuplicate == false)
                {
                    return;
                }

                // ToDo....
            }
            else
            {
                Console.WriteLine();
                int index = 1;
                foreach (var title in duplicatedTitles)
                {
                    Console.WriteLine("(" + index + ") " + title);
                    index++;
                }

                Console.WriteLine("Are these duplicates? (y/n) (y does nothing and requires manual intervention)");
                var userInput = Console.ReadLine()?.Trim().ToLower();
                if (userInput == "n")
                {
                    _config.DuplicatedRecords = _config.DuplicatedRecords.Append(new DuplicatedRecord
                    {
                        Titles = duplicatedTitles,
                        IsDuplicate = false,
                    });
                }
                // ToDo...
                // do something with duplicates...
            }

            File.WriteAllText(_configFileName, JsonConvert.SerializeObject(_config));
        }

        public sealed class DuplicatesConfig
        {
            public IEnumerable<DuplicatedRecord> DuplicatedRecords { get; set; } = [];
        };

        public sealed class DuplicatedRecord
        {
            public IEnumerable<string> Titles { get; set; }
            public bool? IsDuplicate { get; set; }
        }
    }
}
