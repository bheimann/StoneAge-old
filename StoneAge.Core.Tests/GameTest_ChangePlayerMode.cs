using System;
using NUnit.Framework;
using StoneAge.Core.Models.Players;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_ChangePlayerMode : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_change_PlayerMode_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ChangePlayerMode(playerId, PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_change_PlayerMode_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ChangePlayerMode(playerId, PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_change_PlayerMode_for_a_non_existent_player()
        {
            var result = game.ChangePlayerMode(Guid.NewGuid(), PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsFalse(result.Successful);
        }
    }
}
