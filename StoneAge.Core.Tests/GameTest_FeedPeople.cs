using System;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_FeedPeople : GameTestBase
    {
        [Test]
        public void Can_feed_people_in_FeedPeople_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);
            var preFeedFood = game.RequestPlayerStats(player1).Value.Food;

            var result = game.FeedPeople(player1);

            Assert.IsTrue(result.Successful);
            var postFeedFood = game.RequestPlayerStats(player1).Value.Food;
            Assert.AreEqual(preFeedFood, postFeedFood + 5);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_feed_people_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.FeedPeople(player1);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_feed_people_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);

            var result = game.FeedPeople(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }
}
