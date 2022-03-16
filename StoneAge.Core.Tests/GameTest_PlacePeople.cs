using System;
using System.Linq;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_PlacePeople : GameTestBase
    {
        [Test]
        public void Can_place_people_in_PlayersPlacePeople_Phase()
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_place_people_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.PlacePeople(playerId, 1, BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_place_people_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(Guid.NewGuid(), 1, BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_place_more_People_that_player_has()
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(Guid.NewGuid(), 10, BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_place_0_People_anywhere()
        {
            SetUpStandard2PlayerGame();

            var boardSpaces = Enum.GetValues(typeof(BoardSpace)).Cast<BoardSpace>();

            foreach (var boardSpace in boardSpaces)
            {
                var result = game.PlacePeople(Guid.NewGuid(), 0, boardSpace);

                Assert.IsFalse(result.Successful);
            }
        }

        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(3, false)]
        public void Must_place_2_people_in_Hut(int peopleCount, bool expected)
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, peopleCount, BoardSpace.Hut);

            Assert.AreEqual(expected, result.Successful);
        }

        [TestCase(BoardSpace.ToolMaker)]
        [TestCase(BoardSpace.Field)]
        [TestCase(BoardSpace.CivilizationCardSlot1)]
        [TestCase(BoardSpace.CivilizationCardSlot2)]
        [TestCase(BoardSpace.CivilizationCardSlot3)]
        [TestCase(BoardSpace.CivilizationCardSlot4)]
        [TestCase(BoardSpace.BuildingTileSlot1)]
        [TestCase(BoardSpace.BuildingTileSlot2)]
        [TestCase(BoardSpace.BuildingTileSlot3)]
        [TestCase(BoardSpace.BuildingTileSlot4)]
        public void Can_only_place_1_person_in_locations(BoardSpace space)
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, 1, space);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(BoardSpace.ToolMaker)]
        [TestCase(BoardSpace.Field)]
        [TestCase(BoardSpace.CivilizationCardSlot1)]
        [TestCase(BoardSpace.CivilizationCardSlot2)]
        [TestCase(BoardSpace.CivilizationCardSlot3)]
        [TestCase(BoardSpace.CivilizationCardSlot4)]
        [TestCase(BoardSpace.BuildingTileSlot1)]
        [TestCase(BoardSpace.BuildingTileSlot2)]
        [TestCase(BoardSpace.BuildingTileSlot3)]
        [TestCase(BoardSpace.BuildingTileSlot4)]
        public void Cannot_place_2_people_in_locations(BoardSpace space)
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, 2, space);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_place_person_where_already_full()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.Field);

            var result = game.PlacePeople(player2, 1, BoardSpace.Field);

            Assert.IsFalse(result.Successful);
        }

        [TestCase(BoardSpace.Forest)]
        [TestCase(BoardSpace.ClayPit)]
        [TestCase(BoardSpace.Quarry)]
        [TestCase(BoardSpace.River)]
        public void Can_place_7_people_in_locations(BoardSpace space)
        {
            SetupUltimate2PlayerGame();

            var result = game.PlacePeople(player1, 7, space);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Can_place_10_people_in_HuntingGrounds()
        {
            SetupUltimate2PlayerGame();

            var result = game.PlacePeople(player1, 10, BoardSpace.HuntingGrounds);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(BoardSpace.Forest)]
        [TestCase(BoardSpace.ClayPit)]
        [TestCase(BoardSpace.Quarry)]
        [TestCase(BoardSpace.River)]
        public void Cannot_place_8_people_in_locations(BoardSpace space)
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, 8, space);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Must_place_people_in_the_correct_order()
        {
            SetUpStandard2PlayerGame();

            var result1 = game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);
            var result2 = game.PlacePeople(player2, 1, BoardSpace.HuntingGrounds);
            var result_invalid = game.PlacePeople(player2, 1, BoardSpace.Forest);
            var result4 = game.PlacePeople(player1, 1, BoardSpace.Forest);
            var result5 = game.PlacePeople(player2, 1, BoardSpace.Forest);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsFalse(result_invalid.Successful);
            Assert.IsTrue(result4.Successful);
            Assert.IsTrue(result5.Successful);
        }

        [Test]
        public void Cannot_place_people_in_the_same_place_twice()
        {
            SetUpStandard2PlayerGame();

            var result1 = game.PlacePeople(player1, 1, BoardSpace.Forest);
            var result2 = game.PlacePeople(player2, 1, BoardSpace.HuntingGrounds);
            var result_invalid = game.PlacePeople(player1, 1, BoardSpace.Forest);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsFalse(result_invalid.Successful);
        }

        [Test]
        public void Can_place_people_in_the_HuntingGrounds_twice()
        {
            SetUpStandard2PlayerGame();

            var result1 = game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);
            var result2 = game.PlacePeople(player2, 1, BoardSpace.HuntingGrounds);
            var result3 = game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
        }

        [Test]
        public void Cannot_place_more_people_than_player_has()
        {
            SetUpStandard2PlayerGame();

            var result = game.PlacePeople(player1, 10, BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }
    }
}
