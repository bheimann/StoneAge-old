using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Cards;
using StoneAge.Core.Exceptions;

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

    [TestFixture]
    public class GameBoardTest_GetCardFromSpace
    {
        [Test]
        public void Can_get_cards_from_CivilizationCardSlot1()
        {
            var board = new StandardGameBoard();
            board.CardSlot1 = Card.BF11;

            var card = board.GetCardFromSpace(BoardSpace.CivilizationCardSlot1);

            Assert.AreEqual(Card.BF11, card);
        }

        [Test]
        public void Can_get_cards_from_CivilizationCardSlot2()
        {
            var board = new StandardGameBoard();
            board.CardSlot2 = Card.BF11;

            var card = board.GetCardFromSpace(BoardSpace.CivilizationCardSlot2);

            Assert.AreEqual(Card.BF11, card);
        }

        [Test]
        public void Can_get_cards_from_CivilizationCardSlot3()
        {
            var board = new StandardGameBoard();
            board.CardSlot3 = Card.BF11;

            var card = board.GetCardFromSpace(BoardSpace.CivilizationCardSlot3);

            Assert.AreEqual(Card.BF11, card);
        }

        [Test]
        public void Can_get_cards_from_CivilizationCardSlot4()
        {
            var board = new StandardGameBoard();
            board.CardSlot4 = Card.BF11;

            var card = board.GetCardFromSpace(BoardSpace.CivilizationCardSlot4);

            Assert.AreEqual(Card.BF11, card);
        }

        [Test]
        public void Cannot_get_cards_from_other_slots()
        {
            var board = new StandardGameBoard();

            Assert.Throws<InvalidSpaceForCardsException>(() => board.GetCardFromSpace(BoardSpace.HuntingGrounds));
        }
    }
}
