using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Cards;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    [Ignore("")]
    public class GameTest_ClaimLotteryResult : GameTestBase
    {
        // TODO: potentially inject the deck of cards to make this easier to test
        [Test]
        public void Can_claim_lottery_result_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
            }
            while (card.CardImmediate != CardImmediate.Lottery);

            for (int i = 6; i >= 1; i--)
            {
                var result = game.ClaimLotteryResult(player1, i);
                if (result.Successful)
                    return;
            }
            Assert.Fail("Should have been able to claim a die.");
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_claim_lottery_result_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ClaimLotteryResult(playerId, 0);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_lottery_result_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.ClaimLotteryResult(Guid.NewGuid(), 0);

            Assert.IsFalse(result.Successful);
        }
    }
}
