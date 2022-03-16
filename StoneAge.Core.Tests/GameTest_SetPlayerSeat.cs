using System;
using NUnit.Framework;
using StoneAge.Core.Models.Players;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_SetPlayerSeat : GameTestBase
    {
        [Test]
        public void Can_set_Player_seat_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_set_Player_seat_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_set_Player_seat_for_a_non_existent_player()
        {
            var result = game.SetPlayerSeat(Guid.NewGuid(), Chair.North);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_set_the_same_Player_seat_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(playerId, Chair.North);
            var result2 = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Can_set_Player_seat_as_long_as_not_selecting_seat_already_in()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(playerId, Chair.North);
            var result2 = game.SetPlayerSeat(playerId, Chair.South);
            var result3 = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
        }

        [Test]
        public void Can_sit_at_previously_set_Player_seat_as_long_as_switching()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(player1, Chair.North);
            var result2 = game.SetPlayerSeat(player2, Chair.East);
            var result3 = game.SetPlayerSeat(player2, Chair.South);
            var result4 = game.SetPlayerSeat(player1, Chair.South);
            var result5 = game.SetPlayerSeat(player1, Chair.East);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsFalse(result4.Successful);
            Assert.IsTrue(result5.Successful);
        }

        [Test]
        public void Can_set_to_Standing_by_multiple_players()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            var player3 = game.AddPlayer().Value;
            var player4 = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(player1, Chair.South);
            var result2 = game.SetPlayerSeat(player2, Chair.North);
            var result3 = game.SetPlayerSeat(player3, Chair.East);
            var result4 = game.SetPlayerSeat(player4, Chair.West);
            var result5 = game.SetPlayerSeat(player1, Chair.Standing);
            var result6 = game.SetPlayerSeat(player2, Chair.Standing);
            var result7 = game.SetPlayerSeat(player3, Chair.Standing);
            var result8 = game.SetPlayerSeat(player4, Chair.Standing);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
            Assert.IsTrue(result5.Successful);
            Assert.IsTrue(result6.Successful);
            Assert.IsTrue(result7.Successful);
            Assert.IsTrue(result8.Successful);
        }
    }
}
