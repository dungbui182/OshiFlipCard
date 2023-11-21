using System.Collections.Generic;
using System.Linq;

namespace GrandDreams.Core.Utilities
{
    public static class DebugExtensions
    {
        public static string LogKey<T1, T2>(this Dictionary<T1, T2> dictionary)
        {
            string strLog = string.Join(",", dictionary.Select(x => x.Key.ToString()).ToArray());
            return strLog;
        }

        public static string LogValue<T1, T2>(this Dictionary<T1, T2> dictionary)
        {
            string strLog = string.Join(",", dictionary.Select(x => x.Value.ToString()).ToArray());
            return strLog;
        }

        public static string ToLogString<T>(this T[] array, string separateString = ",")
        {
            return string.Join(separateString, array.Select(x => x.ToString()).ToArray());
        }

        public static string ToLogString<T>(this IEnumerable<T> array, string separateString = ",")
        {
            return string.Join(separateString, array.Select(x => x.ToString()).ToArray());
        }
    }
}
