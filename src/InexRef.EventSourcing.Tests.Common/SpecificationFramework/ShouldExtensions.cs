#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2019 INEX Solutions Ltd
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
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

        public static void ShouldContainOnlyItemsWithTypes(this IEnumerable<object> actual, params Type[] expected)
        {
            var comparisonResult = actual.Select(item => item.GetType()).CheckContainsOnly(expected);

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

        public static void ShouldContain<TEnumerable, TItem>(this IEnumerable<TEnumerable> collection, Func<TItem, bool> criteria)
            where TItem : TEnumerable
        {
            var list = collection.ToList();
            var matchingTypes = list.OfType<TItem>();
            var actualMatches = matchingTypes.Where(criteria);

            if (!actualMatches.Any())
            {
                var msg = $"No items in the collection met the specified criteria.\nActual items were:\n{list.ToBulletList()}\n\n";
                throw new SpecificationException(msg);
            }
        }

        public static void ShouldContainItemWithType<T>(this IEnumerable<object> actual)
        {
            ShouldContainItemWithType(actual, typeof(T));
        }

        public static void ShouldContainItemWithType(this IEnumerable<object> actual, Type expected)
        {
            if (!actual.Any(evt => evt.GetType() == expected))
            {
                var msg = $"Expected list to contain: {expected.Namespace}, but didn't";
                throw new SpecificationException(msg);
            }
        }
    }
}
