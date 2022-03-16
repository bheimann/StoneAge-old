using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Tools;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    [Ignore("")]
    public class GameTest_TapTool : GameTestBase
    {
        [Test]
        public void Can_tap_tool_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.ToolMaker);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            game.UseActionOfPeople(player1, BoardSpace.ToolMaker);
            game.UseActionOfPeople(player1, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.TapTool(player1, new List<Tool> { Tool.Plus1 });

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_tap_tool_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.TapTool(playerId, new List<Tool> { Tool.Plus1 });

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_tap_tool_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.TapTool(Guid.NewGuid(), new[] { Tool.Plus1 });

            Assert.IsFalse(result.Successful);
        }
    }
}
