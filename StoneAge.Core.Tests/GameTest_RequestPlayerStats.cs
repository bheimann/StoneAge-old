using System;
using NUnit.Framework;
using StoneAge.Core.Models.Players;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RequestPlayerStats : GameTestBase
    {
        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Can_request_Player_stats_in_any_Phase(GamePhase phase)
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);
            game.Phase = phase;

            var result = game.RequestPlayerStats(player1, chair);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_a_non_existent_player()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(Guid.NewGuid(), chair);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_an_empty_Chair()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(player1, Chair.South);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_Standing_Chair()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(player1, Chair.Standing);

            Assert.IsFalse(result.Successful);
        }
    }
}
