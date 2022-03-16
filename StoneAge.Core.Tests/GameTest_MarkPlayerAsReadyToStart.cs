using System;
using NUnit.Framework;
using StoneAge.Core.Models.Players;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_MarkPlayerAsReadyToStart : GameTestBase
    {
        [Test]
        public void Can_mark_Player_ready_to_start_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_mark_Player_ready_to_start_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_mark_Player_ready_to_start_for_a_non_existent_player()
        {
            var result = game.MarkPlayerAsReadyToStart(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_mark_the_same_Player_ready_to_start_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.MarkPlayerAsReadyToStart(playerId);
            var result2 = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Marking_all_Players_ready_will_advance_for_2_or_more_total()
        {
            var player1 = game.AddPlayer().Value;

            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);
            var result1 = game.MarkPlayerAsReadyToStart(player1);
            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);

            var player2 = game.AddPlayer().Value;
            var result2 = game.MarkPlayerAsReadyToStart(player2);
            WaitForBoardSetupToComplete();

            Assert.AreNotEqual(GamePhase.ChoosePlayers, game.Phase);
            Assert.AreEqual(GamePhase.PlayersPlacePeople, game.Phase);
            Assert.IsFalse(game.ClaimPlayerColor(player1, PlayerColor.NotChosenYet).Successful);
        }
    }
}
