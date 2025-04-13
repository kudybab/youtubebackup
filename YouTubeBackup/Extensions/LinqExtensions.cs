namespace YouTubeBackup.Extensions
{
    public static class LinqExtensions
    {
        public static bool ContainsAll(this IEnumerable<string> values1, IEnumerable<string> values2)
        {
            if (values1 == null || values2 == null)
            {
                throw new NullReferenceException();
            }

            if (!values1.Any() || !values2.Any())
            {
                return false;
            }

            foreach (var value1 in values1)
            {
                if (!values2.Contains(value1, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Contains(this IEnumerable<string> values, string value, StringComparison stringComparison)
        {
            if (values == null)
            {
                throw new NullReferenceException();
            }

            if (!values.Any())
            {
                return false;
            }

            foreach (var val in values)
            {
                if (string.Equals(val, value, stringComparison))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
