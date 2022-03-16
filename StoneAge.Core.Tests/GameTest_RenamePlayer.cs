using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_RenamePlayer : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_rename_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RenamePlayer(playerId, "James Dean");

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_rename_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RenamePlayer(playerId, "Jimmy Dean");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_any_Player_in_any_order()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result = game.RenamePlayer(player1, "Ayn Rand");

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_rename_a_non_existent_player()
        {
            var result = game.RenamePlayer(Guid.NewGuid(), "Billy Bob Roberts");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_a_removed_player()
        {
            var playerId = game.AddPlayer().Value;
            game.RemovePlayer(playerId);

            var result = game.RenamePlayer(playerId, "Robert Barber");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RenamePlayer(playerId, "Marco Polo");
            var result2 = game.RenamePlayer(playerId, "Socrates");

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_null()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, null);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_EmptyString()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, string.Empty);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_all_spaces()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, "  \t  \t  \t  ");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_with_newline_or_carriage_return()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, "\nDarth Vader\r");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_Player_up_to_100_characters()
        {
            var playerId = game.AddPlayer().Value;
            var longName = new string('c', 100);
            var longerName = new string('c', 101);

            var result1 = game.RenamePlayer(playerId, longName);
            var result2 = game.RenamePlayer(playerId, longerName);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }
}
