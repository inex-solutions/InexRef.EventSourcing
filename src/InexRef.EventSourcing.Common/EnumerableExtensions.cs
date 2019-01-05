using System.Collections.Generic;
using System.Linq;

namespace InexRef.EventSourcing.Common
{
    public static class EnumerableExtensions
    {
        public static ComparisonResult<T> CheckContainsOnly<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            var originalActualList = actual.ToList();
            var originalExpectedList = expected.ToList();

            var actualList = originalActualList.ToList();
            var expectedList = originalExpectedList.ToList();

            var unexpected = new List<T>();

            foreach (var actualItem in actualList)
            {
                if (!expectedList.Remove(actualItem))
                {
                    unexpected.Add(actualItem);
                }
            }

            return new ComparisonResult<T>(originalExpectedList, originalActualList, expectedList, unexpected);
        }

        public class ComparisonResult<T>
        {
            public ComparisonResult(IEnumerable<T> expectedList, IEnumerable<T> actualList, IEnumerable<T> missingItems, IEnumerable<T> unexpectedItems)
            {
                ExpectedItems = expectedList.ToArray();
                ActualItems = actualList.ToArray();
                MissingItems = missingItems.ToArray();
                UnexpectedItems = unexpectedItems.ToArray();
            }

            public T[] UnexpectedItems { get; set; }

            public T[] MissingItems { get; set; }

            public T[] ActualItems { get; set; }

            public T[] ExpectedItems { get; set; }

            public bool CollectionsMatch => !MissingItems.Any() && !UnexpectedItems.Any();
        }
    }
}