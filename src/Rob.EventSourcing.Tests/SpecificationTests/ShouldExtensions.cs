using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rob.EventSourcing.Tests.SpecificationTests
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
    }
}
