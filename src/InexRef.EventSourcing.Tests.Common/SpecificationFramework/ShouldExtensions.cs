using System.Collections.Generic;
using System.Linq;
using InexRef.EventSourcing.Common;

namespace InexRef.EventSourcing.Tests.Common.SpecificationFramework
{
    public static class ShouldExtensions
    {
        public static void ShouldHaveACountOf<T>(this IEnumerable<T> items, int expectedCount)
        {
            var itemList = items.ToList();
            if (itemList.Count != expectedCount)
            {
                throw new SpecificationException(
                    $"Expected {expectedCount} item(s) in the collection, but was {itemList.Count}:\n " +
                    string.Join("\n ", itemList));
            }
        }

        public static void ShouldContainOnly<T>(this IEnumerable<T> actual, IEnumerable<T> expected)
        {
            var comparisonResult = actual.CheckContainsOnly(expected);

            if (!comparisonResult.CollectionsMatch)
            {
                var msg =
                    $"Expected list to contain: {comparisonResult.ExpectedItems.ToBulletList()}\n" +
                    $"but contained: {comparisonResult.ActualItems.ToBulletList()}\n" +
                    $"Missing: {comparisonResult.MissingItems.ToBulletList()}\n" +
                    $"Unexpected: {comparisonResult.UnexpectedItems.ToBulletList()}";

                throw new SpecificationException(msg);
            }
        }
    }
}
