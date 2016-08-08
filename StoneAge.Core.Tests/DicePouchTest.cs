using System.Linq;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class DicePouchTest
    {
        [Test]
        public void Rolling0DiceReturnsEmptyDiceResult()
        {
            var pouch = new DicePouch();

            var results = pouch.Roll(0);

            Assert.IsEmpty(results.Summary());
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(9)]
        [TestCase(10)]
        public void Rolling_DiceReturnsDiceResultWith_Dice(int count)
        {
            var pouch = new DicePouch();

            var results = pouch.Roll(count);

            Assert.AreEqual(count, results.Summary().Count());
        }

        [Test]
        public void ShouldHaveAllNumbersAfter36Rolls()
        {
            var pouch = new DicePouch();

            var results = pouch.Roll(36);

            CollectionAssert.Contains(results.Summary(), 1);
            CollectionAssert.Contains(results.Summary(), 2);
            CollectionAssert.Contains(results.Summary(), 3);
            CollectionAssert.Contains(results.Summary(), 4);
            CollectionAssert.Contains(results.Summary(), 5);
            CollectionAssert.Contains(results.Summary(), 6);
        }
    }
}
