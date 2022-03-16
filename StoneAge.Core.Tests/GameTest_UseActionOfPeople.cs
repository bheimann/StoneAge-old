using System;
using System.Collections.Generic;
using NUnit.Framework;
using StoneAge.Core.Models;

namespace StoneAge.Core.Tests.Models
{
    [TestFixture]
    public class GameTest_UseActionOfPeople : GameTestBase
    {
        [Test]
        public void Can_use_people_action_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_use_people_action_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.UseActionOfPeople(playerId, BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_use_people_action_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);

            var result = game.UseActionOfPeople(Guid.NewGuid(), BoardSpace.HuntingGrounds);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_use_people_action_where_Player_has_no_people()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);

            var result = game.UseActionOfPeople(player1, BoardSpace.Forest);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Using_breeding_hut_increase_population_by_1()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 2, BoardSpace.Hut);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 3, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.Hut);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.AreEqual(6, result.Value.PeopleToPlace);
            Assert.AreEqual(6, result.Value.TotalPeople);
        }

        [Test]
        public void Using_field_increase_food_track_by_1()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.Field);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.Field);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.AreEqual(1, result.Value.FoodTrack);
        }

        [Test]
        public void Using_HuntingGrounds_increases_food()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1);
            var preRollFood = stats.Value.Food;
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.Less(preRollFood, result.Value.Food);
        }

        [Test]
        public void Using_Forest_increases_Wood()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.Forest);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1);
            var preRollWood = stats.Value.Wood;
            game.UseActionOfPeople(player1, BoardSpace.Forest);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.Less(preRollWood, result.Value.Wood);
        }

        [Test]
        public void Using_ClayPit_increases_Brick()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.ClayPit);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1);
            var preRollBrick = stats.Value.Brick;
            game.UseActionOfPeople(player1, BoardSpace.ClayPit);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.Less(preRollBrick, result.Value.Brick);
        }

        [Test]
        public void Using_Quarry_increases_Stone()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.Quarry);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1);
            var preRollStone = stats.Value.Stone;
            game.UseActionOfPeople(player1, BoardSpace.Quarry);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.Less(preRollStone, result.Value.Stone);
        }

        [Test]
        public void Using_River_increases_Gold()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 2, BoardSpace.Hut);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 1, BoardSpace.Field);
            game.PlacePeople(player1, 2, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.Hut);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.Field);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            game.FeedPeople(player1);
            game.FeedPeople(player2);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 6, BoardSpace.River);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1);
            var preRollGold = stats.Value.Gold;
            game.UseActionOfPeople(player1, BoardSpace.River);

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.Less(preRollGold, result.Value.Gold);
        }

        [Test]
        public void Using_ToolMaker_adds_single_tool()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.ToolMaker);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
            var stats = game.RequestPlayerStats(player1).Value;

            var result = game.UseActionOfPeople(player1, BoardSpace.ToolMaker);

            Assert.IsTrue(result.Successful);
        }

        // TODO: test upgrading to occur on untapped tools first, will need to be able to tap tools first

        [Test]
        public void Test_increasing_Tools_to_all_values_of_4()
        {
            SetUpStandard2PlayerGame();
            for (int i = 0; i < 6; i++)
            {
                game.PlacePeople(player1, 1, BoardSpace.ToolMaker);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot2);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot3);
                game.UseActionOfPeople(player1, BoardSpace.ToolMaker);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                game.PayForCard(player1, new Dictionary<Resource, int>());
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot2);
                game.PayForCard(player1, new Dictionary<Resource, int>());
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot3);
                game.PayForCard(player1, new Dictionary<Resource, int>());
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                Assert.AreEqual(GamePhase.FeedPeople, game.Phase);
                game.FeedPeople(player1);
                game.FeedPeople(player2);

                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.ToolMaker);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot2);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot3);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.ToolMaker);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                game.PayForCard(player1, new Dictionary<Resource, int>());
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot2);
                game.PayForCard(player1, new Dictionary<Resource, int>());
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot3);
                game.PayForCard(player1, new Dictionary<Resource, int>());

                Assert.AreEqual(GamePhase.FeedPeople, game.Phase);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }

            var stats = game.RequestPlayerStats(player1).Value;
            Assert.AreEqual(4, stats.UntappedTools[0].Value);
            Assert.AreEqual(4, stats.UntappedTools[1].Value);
            Assert.AreEqual(4, stats.UntappedTools[2].Value);
        }

        [TestCase(BoardSpace.CivilizationCardSlot1)]
        [TestCase(BoardSpace.CivilizationCardSlot2)]
        [TestCase(BoardSpace.CivilizationCardSlot3)]
        [TestCase(BoardSpace.CivilizationCardSlot4)]
        public void Using_civilization_card_slots_allows_for_payment_of_cards(BoardSpace cardSlot)
        {
            SetupUltimate2PlayerGame();
            game.PlacePeople(player1, 1, cardSlot);
            game.PlacePeople(player2, 10, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 9, BoardSpace.HuntingGrounds);

            var result = game.UseActionOfPeople(player1, cardSlot);

            Assert.IsTrue(result.Successful);

            int cost = (int)cardSlot - (int)BoardSpace.CivilizationCardSlot1 + 1;
            var payResult = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, cost } });
            Assert.IsTrue(payResult.Successful);
        }

        // TODO: building tiles

        [Test]
        public void Cannot_use_actions_before_current_player_finishes()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 2, BoardSpace.Hut);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 1, BoardSpace.Field);
            game.PlacePeople(player1, 2, BoardSpace.HuntingGrounds);
            var result1 = game.UseActionOfPeople(player1, BoardSpace.Hut);
            var result_fail1 = game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            var result2 = game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            var result3 = game.UseActionOfPeople(player1, BoardSpace.Field);
            var result4 = game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            game.FeedPeople(player1);
            game.FeedPeople(player2);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 6, BoardSpace.HuntingGrounds);
            var result_fail2 = game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            var result5 = game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            var result6 = game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result_fail1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
            Assert.IsFalse(result_fail2.Successful);
            Assert.IsTrue(result5.Successful);
            Assert.IsTrue(result6.Successful);
        }
    }
}
