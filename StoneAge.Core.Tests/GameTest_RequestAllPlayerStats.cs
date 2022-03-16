using System;
using System.Linq;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RequestAllPlayerStats : GameTestBase
    {
        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Can_request_all_Player_stats_in_any_Phase(GamePhase phase)
        {
            player1 = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestAllPlayerStats(player1);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_all_Player_stats_for_a_non_existent_player()
        {
            player1 = game.AddPlayer().Value;

            var result = game.RequestAllPlayerStats(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Requesting_all_Player_stats_returns_for_multiple_players()
        {
            player1 = game.AddPlayer().Value;
            game.AddPlayer();
            game.AddPlayer();
            game.AddPlayer();

            var result = game.RequestAllPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.AreEqual(4, result.Value.Count());
        }
    }
}
