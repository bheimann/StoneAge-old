using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameBoardTest
    {
        [Test]
        public void Sets_initial_values()
        {
            var board = new StandardGameBoard();

            Assert.AreEqual(36, board.CardDeck.Count);

            Assert.AreEqual(7, board.HutStack1.Remaining);
            Assert.AreEqual(7, board.HutStack2.Remaining);
            Assert.AreEqual(7, board.HutStack3.Remaining);
            Assert.AreEqual(7, board.HutStack4.Remaining);

            Assert.AreEqual(20, board.WoodAvailable);
            Assert.AreEqual(16, board.BrickAvailable);
            Assert.AreEqual(12, board.StoneAvailable);
            Assert.AreEqual(10, board.GoldAvailable);

            Assert.AreEqual(12, board.Tool1or2Available);
            Assert.AreEqual(6, board.Tool3or4Available);
        }
    }
}
