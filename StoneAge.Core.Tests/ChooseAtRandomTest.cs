using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class ChooseAtRandomTest
    {
        [Test]
        [ExpectedException]
        public void Throws_when_list_is_empty()
        {
            var list = new List<int>();

            Assert.IsNotNull(list.ChooseAtRandom());
        }

        [Test]
        public void Picks_1()
        {
            var list = new List<int>
            {
                42,
            };

            Assert.AreEqual(42, list.ChooseAtRandom());
        }

        [Test]
        public void Shuffles_10_numbers()
        {
            var list = new List<int>
            {
                0,1,2,3,4,5,6,7,8,9
            };
            Assert.That(list.ChooseAtRandom(), Is.AtLeast(0).And.AtMost(9));
        }
    }
}
