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

            public T[] UnexpectedItems { get; }

            public T[] MissingItems { get; }

            public T[] ActualItems { get; }

            public T[] ExpectedItems { get; }

            public bool CollectionsMatch => !MissingItems.Any() && !UnexpectedItems.Any();
        }
    }
}