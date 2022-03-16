using System;
using NUnit.Framework;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_AddPlayer : GameTestBase
    {
        [Test]
        public void Can_add_Player_in_ChoosePlayers_Phase()
        {
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.AddPlayer();

            Assert.IsTrue(result.Successful);
            Assert.AreNotEqual(default(Guid), result.Value);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_add_Player_in_Phase(GamePhase phase)
        {
            game.Phase = phase;

            var result = game.AddPlayer();

            Assert.IsFalse(result.Successful);
            Assert.AreEqual(default(Guid), result.Value);
        }

        [Test]
        public void Can_add_up_to_4_Players()
        {
            var result1 = game.AddPlayer();
            var result2 = game.AddPlayer();
            var result3 = game.AddPlayer();
            var result4 = game.AddPlayer();

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
        }

        [Test]
        public void Cannot_add_5_Players()
        {
            var result1 = game.AddPlayer();
            var result2 = game.AddPlayer();
            var result3 = game.AddPlayer();
            var result4 = game.AddPlayer();
            var result5 = game.AddPlayer();

            Assert.AreEqual(true, result1.Successful);
            Assert.AreEqual(true, result2.Successful);
            Assert.AreEqual(true, result3.Successful);
            Assert.AreEqual(true, result4.Successful);
            Assert.AreEqual(false, result5.Successful);
            Assert.AreEqual(default(Guid), result5.Value);
        }
    }
}
