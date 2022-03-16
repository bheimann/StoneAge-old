using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RemovePlayer : GameTestBase
    {
        [Test]
        public void Can_remove_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.RemovePlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_remove_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RemovePlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_remove_all_4_Players_in_any_order()
        {
            var ids = new List<Guid>();
            var player1 = game.AddPlayer().Value;
            ids.Add(player1);
            var player2 = game.AddPlayer().Value;
            ids.Add(player2);
            var player3 = game.AddPlayer().Value;
            ids.Add(player3);
            var player4 = game.AddPlayer().Value;
            ids.Add(player4);
            ids.Shuffle();

            foreach (var id in ids)
            {
                var result = game.RemovePlayer(id);
                Assert.IsTrue(result.Successful);
            }
        }

        [Test]
        public void Cannot_remove_a_non_existent_player()
        {
            var result = game.RemovePlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_remove_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RemovePlayer(playerId);
            var result2 = game.RemovePlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }
}
