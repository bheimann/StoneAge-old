using NUnit.Framework;
using StoneAge.Core.Exceptions;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class DiceResultTest
    {
        [Test]
        public void SummaryReturnsEmptyByDefault()
        {
            var diceResult = new DiceResult(new int[0]);
            var orderedDice = diceResult.Summary();

            Assert.IsNotNull(orderedDice);
            Assert.IsEmpty(orderedDice);
            Assert.AreEqual(0, diceResult.Sum());
        }

        [TestCase(0)]
        [TestCase(7)]
        public void InvalidValuesThrowInvadidDieNumberException(int value)
        {
            Assert.Throws<InvalidDieNumberException>(() => new DiceResult(new[] { value }));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void SingleDie(int value)
        {
            var diceResult = new DiceResult(new int[] { value });

            Assert.AreEqual(value, diceResult[0]);
            Assert.AreEqual(value, diceResult.Sum());
        }

        [Test]
        public void OrdersMultipleValuesAdded()
        {
            var diceResult = new DiceResult(new[] { 3, 5, 2, 3 });

            Assert.AreEqual(2, diceResult[0]);
            Assert.AreEqual(3, diceResult[1]);
            Assert.AreEqual(3, diceResult[2]);
            Assert.AreEqual(5, diceResult[3]);

            Assert.AreEqual(13, diceResult.Sum());
        }
    }
}
