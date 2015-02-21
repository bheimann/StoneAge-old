using NUnit.Framework;
using System.Linq;

namespace StoneAge.Core.Tests
{
    [TestFixture]
    public class CardTest
    {
        [Test]
        public void Has28BuildingTiles()
        {
            var count = Card.All.Count();

            Assert.AreEqual(36, count);
        }
    }
}
