using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Players;
using System;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class PlayerTest
    {
        [Test]
        public void NewPlayerStartsWithDefaults()
        {
            var player = new Player();

            Assert.AreNotEqual(default(Guid), player.Id);
            Assert.IsNotNull(player.Name);
            Assert.AreEqual(PlayerColor.NotChosenYet, player.Color);
            Assert.IsNull(player.PlayerBoard);
        }
    }
}
