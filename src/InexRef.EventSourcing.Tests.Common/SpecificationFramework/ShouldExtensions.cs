#region Copyright & License
// The MIT License (MIT)
// 
// Copyright 2017-2018 INEX Solutions Ltd
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

using System.Collections.Generic;
using System.Linq;

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
            var originalActualList = actual.ToList();
            var originalExpectedList = expected.ToList();

            var actualList = actual.ToList();
            var expectedList = expected.ToList();

            var unexpected = new List<T>();

            foreach (var actualItem in actualList)
            {
                if (!expectedList.Remove(actualItem))
                {
                    unexpected.Add(actualItem);
                }
            }

            if (expectedList.Count > 0 || unexpected.Count > 0)
            {
                var msg =
                    $"Expected list to contain: {originalExpectedList.ToBulletList()}\nbut contained: {originalActualList.ToBulletList()}\nMissing: {expectedList.ToBulletList()}\nUnexpected: {unexpected.ToBulletList()}";

                throw new SpecificationException(msg);
            }
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
