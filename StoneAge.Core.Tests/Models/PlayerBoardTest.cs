using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class PlayerBoardTest
    {
        [Test]
        public void NewPlayerBoardStartsWithDefaults()
        {
            var board = new PlayerBoard();

            Assert.AreEqual(12, board.Food);
            Assert.AreEqual(5, board.TotalPeople);

            Assert.AreEqual(0, board.Resources[Resource.Wood]);
            Assert.AreEqual(0, board.Resources[Resource.Brick]);
            Assert.AreEqual(0, board.Resources[Resource.Stone]);
            Assert.AreEqual(0, board.Resources[Resource.Gold]);

            Assert.AreEqual(Tool.None, board.Tools[0]);
            Assert.AreEqual(Tool.None, board.Tools[1]);
            Assert.AreEqual(Tool.None, board.Tools[2]);
        }
    }
}
