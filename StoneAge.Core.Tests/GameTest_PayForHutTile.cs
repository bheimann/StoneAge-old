using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    [Ignore("")]
    public class GameTest_PayForHutTile : GameTestBase
    {
        // TODO: inject the ability to change the huts
        [Test]
        public void Can_pay_for_hut_tile_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.PayForHutTile(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_pay_for_hut_tile_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.PayForHutTile(playerId, new Dictionary<Resource, int>());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_pay_for_hut_tile_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.PayForHutTile(Guid.NewGuid(), new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsFalse(result.Successful);
        }
    }
}
