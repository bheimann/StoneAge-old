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

            Assert.AreEqual(4, board.Players.Count());
        }

        [Test]
        public void NewGameBoardHasDefaultsSetFor2Players()
        {
            var board = new GameBoard(PlayerColor.Blue, PlayerColor.Red);

            Assert.AreEqual(2, board.Players.Count());
        }

        [Test]
        public void NextTests()
        {
            var board = new GameBoard(PlayerColor.Blue, PlayerColor.Red, PlayerColor.Green, PlayerColor.Yellow);

            Assert.AreEqual(PlayerColor.Blue, board.Current.Color);

            board.Next();

            Assert.AreEqual(PlayerColor.Red, board.Current.Color);

            board.Next();

            Assert.AreEqual(PlayerColor.Green, board.Current.Color);

            board.Next();

            Assert.AreEqual(PlayerColor.Yellow, board.Current.Color);

            board.Next();

            Assert.AreEqual(PlayerColor.Blue, board.Current.Color);
        }
    }
}
