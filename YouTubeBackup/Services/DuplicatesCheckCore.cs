using YouTubeBackup.Globals;

namespace YouTubeBackup.Services
{
    public class DuplicatesCheckCore
    {
        public List<List<string>> FindDuplicateSongs(List<string> songTitles)
        {
            List<List<string>> duplicateGroups = new List<List<string>>();
            HashSet<int> visited = new HashSet<int>();

            for (int i = 0; i < songTitles.Count; i++)
            {
                if (visited.Contains(i)) continue;

                List<string> currentGroup = new List<string> { songTitles[i] };

                for (int j = i + 1; j < songTitles.Count; j++)
                {
                    if (visited.Contains(j)) continue;

                    double similarity = CalculateSimilarity(songTitles[i], songTitles[j]);
                    double dynamicThreshold = GetDynamicThreshold(songTitles[i], songTitles[j]);

                    if (similarity >= dynamicThreshold)
                    {
                        currentGroup.Add(songTitles[j]);
                        visited.Add(j);
                    }
                }

                if (currentGroup.Count > 1)
                {
                    duplicateGroups.Add(currentGroup);
                }
            }

            return duplicateGroups;
        }

        static double GetDynamicThreshold(string s1, string s2)
        {
            double baseThreshold = GlobalConfig.DUPLICATE_CHECK_BASE_THRESHOLD;
            int minLength = Math.Min(s1.Length, s2.Length);
            if (minLength < 20) return GlobalConfig.DUPLICATE_CHECK_BASE_THRESHOLD_SHORT_TITLES_BELOW_20_CHARACTERS; 

            return baseThreshold;
        }

        static double CalculateSimilarity(string s1, string s2)
        {
            s1 = s1.ToLower().Trim();
            s2 = s2.ToLower().Trim();

            int distance = LevenshteinDistance(s1, s2);
            int maxLength = Math.Max(s1.Length, s2.Length);
            return 1.0 - (double)distance / maxLength;
        }

        static int LevenshteinDistance(string s1, string s2)
        {
            int len1 = s1.Length;
            int len2 = s2.Length;
            int[,] dp = new int[len1 + 1, len2 + 1];

            for (int i = 0; i <= len1; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= len2; j++)
                dp[0, j] = j;

            for (int i = 1; i <= len1; i++)
            {
                for (int j = 1; j <= len2; j++)
                {
                    int cost = (s1[i - 1] == s2[j - 1]) ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + cost
                    );
                }
            }

            return dp[len1, len2];
        }
    }
}
