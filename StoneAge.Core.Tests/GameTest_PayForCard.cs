using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_PayForCard : GameTestBase
    {
        [Test]
        public void Can_pay_for_card_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);
            game.UseActionOfPeople(player1, BoardSpace.Forest);
            var prePayWood = game.RequestPlayerStats(player1).Value.Wood;
            game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);

            var result = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsTrue(result.Successful);
            var postPayWood = game.RequestPlayerStats(player1).Value.Wood;
            Assert.AreEqual(prePayWood - 1, postPayWood);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_pay_for_card_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.PayForCard(playerId, new Dictionary<Resource, int>());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_pay_for_card_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);
            game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);

            var result = game.PayForCard(Guid.NewGuid(), new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_pay_for_card_when_player_is_not_buying_a_card()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            game.UseActionOfPeople(player1, BoardSpace.Forest);

            var result = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsFalse(result.Successful);
        }

        [Test]
        [Ignore("")]
        public void Must_pay_full_amount()
        {
            // TODO: test not paying enough
        }

        [Test]
        [Ignore("")]
        public void Can_pay_mixed_resources()
        {
        }

        [Test]
        [Ignore("")]
        public void Can_pay_for_card_slot()
        {
            // TODO: test all 4 card slots
        }

        [Test]
        [Ignore("")]
        public void Paying_nothing_is_same_as_skipping()
        {
            // TODO: test skipping payment
        }
    }
}
