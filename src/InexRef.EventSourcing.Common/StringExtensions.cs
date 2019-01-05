using System.Collections.Generic;
using System.Linq;

namespace InexRef.EventSourcing.Common
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitAndTrim(this string source, string separator)
        {
            return source.Split(separator).Select(s => s.Trim());
        }

        public static string ToBulletList<T>(this IEnumerable<T> list)
        {
            var bulletList = string.Join("\n - ", list);

            if (bulletList.Length > 0)
            {
                bulletList = "\n - " + bulletList;
            }

            return bulletList;
        }
    }
}
