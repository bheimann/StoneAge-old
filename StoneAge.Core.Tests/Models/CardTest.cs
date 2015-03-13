using NUnit.Framework;
using StoneAge.Core.Models;
using System.Linq;

namespace StoneAge.Core.Tests.Models
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
