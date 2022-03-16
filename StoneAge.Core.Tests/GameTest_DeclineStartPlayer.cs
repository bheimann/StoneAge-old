using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_DeclineStartPlayer : GameTestBase
    {
        [Test]
        public void Can_decline_start_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;
            game.RequestStartPlayer(playerId);

            var result = game.DeclineStartPlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_decline_start_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;
            game.RequestStartPlayer(playerId);

            var result = game.DeclineStartPlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_a_non_existent_player()
        {
            var result = game.DeclineStartPlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;
            game.RequestStartPlayer(playerId);

            var result1 = game.DeclineStartPlayer(playerId);
            var result2 = game.DeclineStartPlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_Player_that_has_not_requested_to_be_start_player()
        {
            var playerId1 = game.AddPlayer().Value;
            var playerId2 = game.AddPlayer().Value;
            game.RequestStartPlayer(playerId2);

            var result1 = game.DeclineStartPlayer(playerId1);
            var result2 = game.DeclineStartPlayer(playerId2);

            Assert.IsFalse(result1.Successful);
            Assert.IsTrue(result2.Successful);
        }
    }
}
