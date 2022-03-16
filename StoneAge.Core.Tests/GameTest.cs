using NUnit.Framework;

//TODO: a number of these tests might be able to be cleaned up a little, check if using the game stats simplifies things
namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest : GameTestBase
    {
        [Test]
        public void Starting_Phase_is_ChoosePlayers()
        {
            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);
        }
    }
}
