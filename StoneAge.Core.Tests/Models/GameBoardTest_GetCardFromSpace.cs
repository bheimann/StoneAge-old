using NUnit.Framework;
using StoneAge.Core.Exceptions;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Cards;

namespace StoneAge.Core.Tests.Models
{
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
