using NUnit.Framework;
using StoneAge.Core.Exceptions;
using System.Linq;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class DiceResultTest
    {
        private DiceResult _diceResult;

        [SetUp]
        public void Setup()
        {
            _diceResult = new DiceResult();
        }

        [Test]
        public void SummaryReturnsEmptyBeforeADieIsAdded()
        {
            var orderedDice = _diceResult.Summary();

            Assert.IsNotNull(orderedDice);
            Assert.IsEmpty(orderedDice);
            Assert.AreEqual(0, _diceResult.Sum());
        }

        [TestCase(0)]
        [TestCase(7)]
        public void InvalidValuesThrowInvadidDieNumberException(int value)
        {
            var diceResult = new DiceResult();

            Assert.Throws<InvadidDieNumberException>(() => _diceResult.Add(value));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void SingleDie(int value)
        {
            _diceResult.Add(value);

            Assert.AreEqual(value, _diceResult[0]);
            Assert.AreEqual(value, _diceResult.Sum());
        }

        [Test]
        public void OrdersMultipleValuesAdded()
        {
            _diceResult.Add(3);
            _diceResult.Add(5);
            _diceResult.Add(2);
            _diceResult.Add(3);

            Assert.AreEqual(2, _diceResult[0]);
            Assert.AreEqual(3, _diceResult[1]);
            Assert.AreEqual(3, _diceResult[2]);
            Assert.AreEqual(5, _diceResult[3]);

            Assert.AreEqual(13, _diceResult.Sum());
        }

        [Test]
        public void ReordersUponEachIndexer()
        {
            _diceResult.Add(3);

            Assert.AreEqual(3, _diceResult[0]);
            
            _diceResult.Add(1);

            Assert.AreEqual(1, _diceResult[0]);
            Assert.AreEqual(3, _diceResult[1]);

            _diceResult.Add(2);

            Assert.AreEqual(1, _diceResult[0]);
            Assert.AreEqual(2, _diceResult[1]);
            Assert.AreEqual(3, _diceResult[2]);
        }
    }
}
