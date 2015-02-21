using System.Linq;
using NUnit.Framework;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class GameBoardTest
    {
        [Test]
        public void Has4PlayersByDefaultBlueStarting()
        {
            var board = new GameBoard();

            Assert.AreEqual(4, board.Players.Count());
            Assert.AreEqual(PlayerColor.Blue, board.Current.Color);
        }

        [Test]
        public void HasStartingPlayerFirstPlayerPassedInFor2Players()
        {
            var board = new GameBoard(PlayerColor.Red, PlayerColor.Green);

            Assert.AreEqual(2, board.Players.Count());
            Assert.AreEqual(PlayerColor.Red, board.Current.Color);
        }

        [Test]
        public void AllSpacesAreEmptyInitially()
        {
            var board = new GameBoard();

            foreach (var space in board.Spaces)
            {
                Assert.IsNull(space.Value.HeldBy);
                Assert.IsNull(space.Value.ThinkingOf);
            }
        }
    }

    [TestFixture]
    public class GameBoardTest_Next
    {
        [Test]
        public void PlayersRotate()
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

        [Test]
        public void TemporarilySelectedLocationBecomesPerminantAtEndOfTurn()
        {
            var board = new GameBoard(PlayerColor.Blue);

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            board.Spaces[BoardSpace.Forest1].ThinkingOf = PlayerColor.Blue;

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, PlayerColor.Blue);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            board.Next();

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, PlayerColor.Blue);
        }
    }

    [TestFixture]
    public class GameBoardTest_TryToOccupySpace
    {
        [Test]
        public void TemporarilySelectedLocationBecomesPerminantAtEndOfTurn()
        {
            var board = new GameBoard(PlayerColor.Blue);

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            board.Spaces[BoardSpace.Forest1].ThinkingOf = PlayerColor.Blue;

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, PlayerColor.Blue);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            board.Next();

            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, PlayerColor.Blue);
        }
    }
}
