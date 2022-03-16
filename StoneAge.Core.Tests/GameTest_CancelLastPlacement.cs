using System;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    [Ignore("")]
    public class GameTest_CancelLastPlacement : GameTestBase
    {
        // TODO: this will be very house rule specific, cannot go back more than x action and cannot go back if y players have done anything

        [Test]
        public void Can_cancel_last_placement_in_PlayersPlacePeople_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.PlayersPlacePeople, game.Phase);

            var result = game.CancelLastPlacement(player1);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_cancel_last_placement_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.CancelLastPlacement(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_cancel_last_placement_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);

            var result = game.CancelLastPlacement(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }
}
