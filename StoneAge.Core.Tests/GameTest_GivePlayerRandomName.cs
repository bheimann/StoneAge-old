using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_GivePlayerRandomName : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_give_random_name_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.GivePlayerRandomName(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_give_random_name_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.GivePlayerRandomName(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_give_random_name_to_a_non_existent_player()
        {
            var result = game.GivePlayerRandomName(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }
}
