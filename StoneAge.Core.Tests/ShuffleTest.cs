using NUnit.Framework;
using StoneAge.Core.Models;
using System.Collections.Generic;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class ShuffleTest
    {
        [Test]
        public void Empty_list_does_not_throw()
        {
            var list = new List<int>();

            Assert.IsEmpty(list.Shuffle());
        }

        [Test]
        public void Shuffles_10_numbers()
        {
            var list = new List<int>
            {
                0,1,2,3,4,5,6,7,8,9
            };
            var stringRepresentation = StringRepresentation(list);

            Assert.IsNotEmpty(list.Shuffle());
            Assert.AreNotEqual(stringRepresentation, StringRepresentation(list));
        }

        [Test]
        public void Shuffling_the_same_list_more_than_once_gives_different_results()
        {
            var list = new List<int>
            {
                0,1,2,3,4,5,6,7,8,9
            };
            var stringRepresentation = StringRepresentation(list);

            Assert.AreNotEqual(StringRepresentation(list.Shuffle()), StringRepresentation(list.Shuffle()));
        }

        private static string StringRepresentation(IEnumerable<int> list)
        {
            return string.Join(",", list);
        }
    }
}
