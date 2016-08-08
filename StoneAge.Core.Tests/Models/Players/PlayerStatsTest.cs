using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Players;
using StoneAge.Core.Models.Tools;
using System.Collections.Generic;

namespace StoneAge.Core.Tests.Models.Players
{
    [TestFixture]
    public class PlayerStatsTest
    {
        [Test]
        public void Creation_of_PlayerStats()
        {
            var player = new Player
            {
                Name = "Frank",
                Chair = Chair.West,
                Color = PlayerColor.Red,
                Mode = PlayerMode.ComputerStrategyPeople,
                ReadyToStart = true,
                PlayerBoard = new PlayerBoard
                {
                    Food = 22,
                    FoodTrack = 4,
                    PeopleToPlace = 3,
                    TotalPeople = 9,
                    Score = 33,
                }
            };
            player.PlayerBoard.Resources[Resource.Wood] = 5;
            player.PlayerBoard.Resources[Resource.Brick] = 6;
            player.PlayerBoard.Resources[Resource.Stone] = 7;
            player.PlayerBoard.Resources[Resource.Gold] = 8;

            var stats = new PlayerStats(player);

            Assert.AreEqual("Frank", stats.Name);
            Assert.AreEqual(Chair.West, stats.Chair);
            Assert.AreEqual(PlayerColor.Red, stats.Color);
            Assert.AreEqual(PlayerMode.ComputerStrategyPeople, stats.Mode);
            Assert.AreEqual(true, stats.ReadyToStart);
            Assert.AreEqual(22, stats.Food);
            Assert.AreEqual(4, stats.FoodTrack);
            Assert.AreEqual(3, stats.PeopleToPlace);
            Assert.AreEqual(9, stats.TotalPeople);
            Assert.AreEqual(33, stats.Score);
            Assert.AreEqual(5, stats.Wood);
            Assert.AreEqual(6, stats.Brick);
            Assert.AreEqual(7, stats.Stone);
            Assert.AreEqual(8, stats.Gold);
        }

        [Test]
        public void Sets_PlayerBoard_values_to_defaults()
        {
            var player = new Player();

            var stats = new PlayerStats(player);

            Assert.AreEqual(0, stats.Food);
            Assert.AreEqual(0, stats.FoodTrack);
            Assert.AreEqual(0, stats.PeopleToPlace);
            Assert.AreEqual(0, stats.TotalPeople);
            Assert.AreEqual(0, stats.Score);
            Assert.AreEqual(0, stats.Wood);
            Assert.AreEqual(0, stats.Brick);
            Assert.AreEqual(0, stats.Stone);
            Assert.AreEqual(0, stats.Gold);
            CollectionAssert.AreEquivalent(new List<Tool>(), stats.TappedTools);
            CollectionAssert.AreEquivalent(new List<Tool>(), stats.UntappedTools);
        }

        [Test]
        public void Tools_empty()
        {
            var player = new Player
            {
                PlayerBoard = new PlayerBoard()
            };
            player.PlayerBoard.Tools[0] = Tool.None;
            player.PlayerBoard.Tools[1] = Tool.None;
            player.PlayerBoard.Tools[2] = Tool.None;
            player.PlayerBoard.Tools[0].Used = false;
            player.PlayerBoard.Tools[1].Used = true;
            player.PlayerBoard.Tools[2].Used = false;

            var stats = new PlayerStats(player);

            CollectionAssert.AreEquivalent(new List<Tool>(), stats.TappedTools);
            CollectionAssert.AreEquivalent(new List<Tool>(), stats.UntappedTools);
        }

        [Test]
        public void Tools_all_tapped()
        {
            var player = new Player
            {
                PlayerBoard = new PlayerBoard()
            };
            player.PlayerBoard.Tools[0] = Tool.Plus4;
            player.PlayerBoard.Tools[1] = Tool.Plus3;
            player.PlayerBoard.Tools[2] = Tool.Plus3;
            player.PlayerBoard.Tools[0].Used = true;
            player.PlayerBoard.Tools[1].Used = true;
            player.PlayerBoard.Tools[2].Used = true;

            var stats = new PlayerStats(player);

            var tappedTools = new List<Tool> { Tool.Plus3.Tap(), Tool.Plus3.Tap(), Tool.Plus4.Tap() };
            CollectionAssert.AreEquivalent(tappedTools, stats.TappedTools);
            CollectionAssert.AreEquivalent(new List<Tool>(), stats.UntappedTools);
        }

        [Test]
        public void Tools_all_untapped()
        {
            var player = new Player
            {
                PlayerBoard = new PlayerBoard()
            };
            player.PlayerBoard.Tools[0] = Tool.Plus2;
            player.PlayerBoard.Tools[1] = Tool.Plus2;
            player.PlayerBoard.Tools[2] = Tool.Plus1;
            player.PlayerBoard.Tools[0].Used = false;
            player.PlayerBoard.Tools[1].Used = false;
            player.PlayerBoard.Tools[2].Used = false;

            var stats = new PlayerStats(player);

            CollectionAssert.AreEquivalent(new List<Tool>(), stats.TappedTools);
            var untappedTools = new List<Tool> { Tool.Plus1, Tool.Plus2, Tool.Plus2 };
            CollectionAssert.AreEquivalent(untappedTools, stats.UntappedTools);
        }

        [Test]
        public void Tools_mix_tapped()
        {
            var player = new Player
            {
                PlayerBoard = new PlayerBoard()
            };
            player.PlayerBoard.Tools[0] = Tool.Plus4;
            player.PlayerBoard.Tools[1] = Tool.Plus3;
            player.PlayerBoard.Tools[2] = Tool.Plus2;
            player.PlayerBoard.Tools[0].Used = true;
            player.PlayerBoard.Tools[1].Used = false;
            player.PlayerBoard.Tools[2].Used = true;

            var stats = new PlayerStats(player);

            var tappedTools = new List<Tool> { Tool.Plus2.Tap(), Tool.Plus4.Tap() };
            CollectionAssert.AreEquivalent(tappedTools, stats.TappedTools);
            var untappedTools = new List<Tool> { Tool.Plus3 };
            CollectionAssert.AreEquivalent(untappedTools, stats.UntappedTools);
        }
    }
}
