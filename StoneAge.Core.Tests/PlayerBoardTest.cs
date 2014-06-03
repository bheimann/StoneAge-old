using NUnit.Framework;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class PlayerBoardTest
    {
        [Test]
        public void NewPlayerBoardStartsWithDefaults()
        {
            var board = new PlayerBoard(PlayerColor.Blue);

            Assert.AreEqual(PlayerColor.Blue, board.Color);
            Assert.AreEqual(12, board.Food);
            Assert.AreEqual(5, board.People);

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
