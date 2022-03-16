using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RequestStartPlayer : GameTestBase
    {
        [Test]
        public void Can_request_start_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.RequestStartPlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_request_start_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestStartPlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_start_Player_for_a_non_existent_player()
        {
            var result = game.RequestStartPlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_start_Player_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RequestStartPlayer(playerId);
            var result2 = game.RequestStartPlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }
}
