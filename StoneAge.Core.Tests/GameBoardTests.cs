using System.Linq;
using NUnit.Framework;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class GameBoardTests
    {
        [Test]
        public void NewGameBoardHasDefaultsSet()
        {
            var board = new GameBoard();

            Assert.AreEqual(4, board.Scores.Count());
        }

        [Test]
        public void NewGameBoardHasDefaultsSetFor2Players()
        {
            var board = new GameBoard(PlayerColor.Blue, PlayerColor.Red);

            Assert.AreEqual(2, board.Scores.Count());
        }
    }
}
