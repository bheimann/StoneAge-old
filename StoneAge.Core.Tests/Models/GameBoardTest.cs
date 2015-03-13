using System.Linq;
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
            var board = new GameBoard();

            Assert.AreEqual(36, board.CardDeck.Count);

            Assert.AreEqual(7, board.HutStack1.Count);
            Assert.AreEqual(7, board.HutStack2.Count);
            Assert.AreEqual(7, board.HutStack3.Count);
            Assert.AreEqual(7, board.HutStack4.Count);

            Assert.AreEqual(20, board.WoodAvailable);
            Assert.AreEqual(16, board.BrickAvailable);
            Assert.AreEqual(12, board.StoneAvailable);
            Assert.AreEqual(10, board.GoldAvailable);

            Assert.AreEqual(12, board.Tool1or2Available);
            Assert.AreEqual(6, board.Tool3or4Available);
        }

        // Should the spaces available be here? Probably, just stupid keeping place
        //foreach (var space in board.Spaces)
        //{
        //    Assert.IsNull(space.Value.HeldBy);
        //}
    }

    [TestFixture]
    public class GameBoardTest_Next
    {
        [Test]
        public void PlayersRotate()
        {
            //var board = new GameBoard(PlayerColor.Blue, PlayerColor.Red, PlayerColor.Green, PlayerColor.Yellow);

            //Assert.AreEqual(PlayerColor.Blue, board.Current.Color);

            //board.Next();

            //Assert.AreEqual(PlayerColor.Red, board.Current.Color);

            //board.Next();

            //Assert.AreEqual(PlayerColor.Green, board.Current.Color);

            //board.Next();

            //Assert.AreEqual(PlayerColor.Yellow, board.Current.Color);

            //board.Next();

            //Assert.AreEqual(PlayerColor.Blue, board.Current.Color);
        }

        [Test]
        public void TemporarilySelectedLocationBecomesPerminantAtEndOfTurn()
        {
            //var board = new GameBoard(PlayerColor.Blue);

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            //board.Spaces[BoardSpace.Forest1].ThinkingOf = PlayerColor.Blue;

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, PlayerColor.Blue);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            //board.Next();

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, PlayerColor.Blue);
        }
    }

    [TestFixture]
    public class GameBoardTest_TryToOccupySpace
    {
        [Test]
        public void TemporarilySelectedLocationBecomesPerminantAtEndOfTurn()
        {
            //var board = new GameBoard(PlayerColor.Blue);

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            //board.Spaces[BoardSpace.Forest1].ThinkingOf = PlayerColor.Blue;

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, PlayerColor.Blue);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, null);

            //board.Next();

            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].ThinkingOf, null);
            //Assert.AreEqual(board.Spaces[BoardSpace.Forest1].HeldBy, PlayerColor.Blue);
        }
    }
}
