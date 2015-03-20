using System.Linq;
using NUnit.Framework;
using StoneAge.Core.Models.BuildingTiles;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class BuildingTileTest
    {
        [Test]
        public void Has28BuildingTiles()
        {
            var count = BuildingTile.All.Count();

            Assert.AreEqual(28, count);
        }
        
        [Test]
        public void AssertCalculationsAreCorrect()
        {
            AssertTileHasValues(BuildingTile.WWB1, 10);
            AssertTileHasValues(BuildingTile.WWS1, 11);
            AssertTileHasValues(BuildingTile.WWG1, 12);
            AssertTileHasValues(BuildingTile.BBS1, 13);
            AssertTileHasValues(BuildingTile.BBG1, 14);
            AssertTileHasValues(BuildingTile.SSG1, 16);
            AssertTileHasValues(BuildingTile.WBB1, 11);
            AssertTileHasValues(BuildingTile.WSS1, 13);
            AssertTileHasValues(BuildingTile.BSS1, 14);

            AssertTileHasValues(BuildingTile.WBS1, 12);
            AssertTileHasValues(BuildingTile.WBS2, 12);
            AssertTileHasValues(BuildingTile.WBG1, 13);
            AssertTileHasValues(BuildingTile.WBG2, 13);
            AssertTileHasValues(BuildingTile.WSG1, 14);
            AssertTileHasValues(BuildingTile.WSG2, 14);
            AssertTileHasValues(BuildingTile.BSG1, 15);
            AssertTileHasValues(BuildingTile.BSG2, 15);

            AssertTileHasValues(BuildingTile.C411, 12, 24);
            AssertTileHasValues(BuildingTile.C421, 13, 23);
            AssertTileHasValues(BuildingTile.C431, 15, 21);
            AssertTileHasValues(BuildingTile.C441, 18, 18);

            AssertTileHasValues(BuildingTile.C511, 15, 30);
            AssertTileHasValues(BuildingTile.C521, 16, 29);
            AssertTileHasValues(BuildingTile.C531, 18, 27);
            AssertTileHasValues(BuildingTile.C541, 21, 24);

            AssertTileHasValues(BuildingTile.C171, 3, 42);
            AssertTileHasValues(BuildingTile.C172, 3, 42);
            AssertTileHasValues(BuildingTile.C173, 3, 42);
        }

        public void AssertTileHasValues(BuildingTile tile, int value)
        {
            AssertTileHasValues(tile, value, value);
        }

        public void AssertTileHasValues(BuildingTile tile, int minValue, int maxValue)
        {
            Assert.AreEqual(minValue, tile.MinValue);
            Assert.AreEqual(maxValue, tile.MaxValue);
        }
    }
}
