using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RequestPlayerStats_Self : GameTestBase
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
            player1 = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_a_non_existent_player()
        {
            player1 = game.AddPlayer().Value;

            var result = game.RequestPlayerStats(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }
}
