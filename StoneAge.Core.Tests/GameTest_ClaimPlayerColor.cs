using System;
using NUnit.Framework;
using StoneAge.Core.Models.Players;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_ClaimPlayerColor : GameTestBase
    {
        [Test]
        public void Can_claim_PlayerColor_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_claim_PlayerColor_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_PlayerColor_for_a_non_existent_player()
        {
            var result = game.ClaimPlayerColor(Guid.NewGuid(), PlayerColor.Red);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_the_same_PlayerColor_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(playerId, PlayerColor.Red);
            var result2 = game.ClaimPlayerColor(playerId, PlayerColor.Red);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Can_claim_PlayerColor_as_long_as_switching()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(playerId, PlayerColor.Blue);
            var result2 = game.ClaimPlayerColor(playerId, PlayerColor.Red);
            var result3 = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
        }

        [Test]
        public void Can_claim_previously_claimed_PlayerColor_as_long_as_switching()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(player1, PlayerColor.Blue);
            var result2 = game.ClaimPlayerColor(player2, PlayerColor.Green);
            var result3 = game.ClaimPlayerColor(player2, PlayerColor.Red);
            var result4 = game.ClaimPlayerColor(player1, PlayerColor.Red);
            var result5 = game.ClaimPlayerColor(player1, PlayerColor.Green);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsFalse(result4.Successful);
            Assert.IsTrue(result5.Successful);
        }

        [Test]
        public void Can_claim_NotChosenYet_by_multiple_players()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            var player3 = game.AddPlayer().Value;
            var player4 = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(player1, PlayerColor.Red);
            var result2 = game.ClaimPlayerColor(player2, PlayerColor.Blue);
            var result3 = game.ClaimPlayerColor(player3, PlayerColor.Green);
            var result4 = game.ClaimPlayerColor(player4, PlayerColor.Yellow);
            var result5 = game.ClaimPlayerColor(player1, PlayerColor.NotChosenYet);
            var result6 = game.ClaimPlayerColor(player2, PlayerColor.NotChosenYet);
            var result7 = game.ClaimPlayerColor(player3, PlayerColor.NotChosenYet);
            var result8 = game.ClaimPlayerColor(player4, PlayerColor.NotChosenYet);

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
