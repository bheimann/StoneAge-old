using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Cards;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    [Ignore("")]
    public class GameTest_UseSpecialAction : GameTestBase
    {
        // TODO: potentially inject the deck of cards to make this easier to test
        [Test]
        public void Can_use_special_action_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            for (int i = 0; i < 5; i++)
            {
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.Forest);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            while (card.CardImmediate != CardImmediate.Resources_2);
            // TODO: account for where one of the last 4/5 cards is the 2 resources card
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.UseSpecialAction(player1, SpecialAction.Take2ResourcesCard);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Can_use_special_action_in_FeedPeople_Phase()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            game.RequestStartPlayer(player1);
            game.MarkPlayerAsReadyToStart(player1);
            game.MarkPlayerAsReadyToStart(player2);
            WaitForBoardSetupToComplete();
            for (int i = 0; i < 5; i++)
            {
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.Forest);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            while (card.CardImmediate != CardImmediate.Resources_2);
            // TODO: account for where one of the last 4/5 cards is the 2 resources card
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);

            var result = game.UseSpecialAction(player1, SpecialAction.Take2ResourcesCard);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_use_special_action_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.UseSpecialAction(playerId, SpecialAction.Take2ResourcesCard);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_use_special_action_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.UseSpecialAction(Guid.NewGuid(), SpecialAction.Take2ResourcesCard);

            Assert.IsFalse(result.Successful);
        }
    }
}
