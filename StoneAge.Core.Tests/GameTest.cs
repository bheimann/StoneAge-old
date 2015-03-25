using System.Linq;
using NUnit.Framework;
using StoneAge.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using StoneAge.Core.Models.Players;
using StoneAge.Core.Models.Cards;
using StoneAge.Core.Models.Tools;

//TODO: a number of these tests might be able to be cleaned up a little, check if using the game stats simplifies things
namespace StoneAge.Core.Tests.Models
{
    public abstract class GameTestBase
    {
        protected Game game;

        protected Guid player1;
        protected Guid player2;
        
        [SetUp]
        public void Setup()
        {
            game = new Game();
        }

        protected void SetUpStandard2PlayerGame()
        {
            player1 = game.AddPlayer().Value;
            player2 = game.AddPlayer().Value;
            game.RequestStartPlayer(player1);
            game.MarkPlayerAsReadyToStart(player1);
            game.MarkPlayerAsReadyToStart(player2);
            WaitForBoardSetupToComplete();
            Assert.AreEqual(GamePhase.PlayersPlacePeople, game.Phase);
        }

        protected void SetupUltimate2PlayerGame()
        {
            game = new Game(new UltimatePlayerBoardFactory());
            SetUpStandard2PlayerGame();
        }

        protected void WaitForBoardSetupToComplete()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (!game.IsThinking)
                    break;
                Thread.Sleep(10);
            }
        }
    }

    [TestFixture]
    public class GameTest : GameTestBase
    {
        [Test]
        public void Starting_Phase_is_ChoosePlayers()
        {
            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);
        }
    }

    [TestFixture]
    public class GameTest_AddPlayer : GameTestBase
    {
        [Test]
        public void Can_add_Player_in_ChoosePlayers_Phase()
        {
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.AddPlayer();

            Assert.IsTrue(result.Successful);
            Assert.AreNotEqual(default(Guid), result.Value);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_add_Player_in_Phase(GamePhase phase)
        {
            game.Phase = phase;

            var result = game.AddPlayer();

            Assert.IsFalse(result.Successful);
            Assert.AreEqual(default(Guid), result.Value);
        }

        [Test]
        public void Can_add_up_to_4_Players()
        {
            var result1 = game.AddPlayer();
            var result2 = game.AddPlayer();
            var result3 = game.AddPlayer();
            var result4 = game.AddPlayer();

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
        }

        [Test]
        public void Cannot_add_5_Players()
        {
            var result1 = game.AddPlayer();
            var result2 = game.AddPlayer();
            var result3 = game.AddPlayer();
            var result4 = game.AddPlayer();
            var result5 = game.AddPlayer();

            Assert.AreEqual(true, result1.Successful);
            Assert.AreEqual(true, result2.Successful);
            Assert.AreEqual(true, result3.Successful);
            Assert.AreEqual(true, result4.Successful);
            Assert.AreEqual(false, result5.Successful);
            Assert.AreEqual(default(Guid), result5.Value);
        }
    }

    [TestFixture]
    public class GameTest_RemovePlayer : GameTestBase
    {
        [Test]
        public void Can_remove_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.RemovePlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_remove_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RemovePlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_remove_all_4_Players_in_any_order()
        {
            var ids = new List<Guid>();
            var player1 = game.AddPlayer().Value;
            ids.Add(player1);
            var player2 = game.AddPlayer().Value;
            ids.Add(player2);
            var player3 = game.AddPlayer().Value;
            ids.Add(player3);
            var player4 = game.AddPlayer().Value;
            ids.Add(player4);
            ids.Shuffle();

            foreach (var id in ids)
            {
                var result = game.RemovePlayer(id);
                Assert.IsTrue(result.Successful);
            }
        }

        [Test]
        public void Cannot_remove_a_non_existent_player()
        {
            var result = game.RemovePlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_remove_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RemovePlayer(playerId);
            var result2 = game.RemovePlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }

    [TestFixture]
    public class GameTest_RequestStartPlayer : GameTestBase
    {
        [Test]
        public void Can_request_start_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.RequestStartPlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_request_start_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestStartPlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_start_Player_for_a_non_existent_player()
        {
            var result = game.RequestStartPlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_start_Player_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RequestStartPlayer(playerId);
            var result2 = game.RequestStartPlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }

    [TestFixture]
    public class GameTest_DeclineStartPlayer : GameTestBase
    {
        [Test]
        public void Can_decline_start_Player_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;
            game.RequestStartPlayer(playerId);

            var result = game.DeclineStartPlayer(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_decline_start_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;
            game.RequestStartPlayer(playerId);

            var result = game.DeclineStartPlayer(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_a_non_existent_player()
        {
            var result = game.DeclineStartPlayer(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;
            game.RequestStartPlayer(playerId);

            var result1 = game.DeclineStartPlayer(playerId);
            var result2 = game.DeclineStartPlayer(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Cannot_decline_start_Player_for_Player_that_has_not_requested_to_be_start_player()
        {
            var playerId1 = game.AddPlayer().Value;
            var playerId2 = game.AddPlayer().Value;
            game.RequestStartPlayer(playerId2);

            var result1 = game.DeclineStartPlayer(playerId1);
            var result2 = game.DeclineStartPlayer(playerId2);

            Assert.IsFalse(result1.Successful);
            Assert.IsTrue(result2.Successful);
        }
    }

    [TestFixture]
    public class GameTest_ClaimPlayerColor : GameTestBase
    {
        [Test]
        public void Can_claim_PlayerColor_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_claim_PlayerColor_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_PlayerColor_for_a_non_existent_player()
        {
            var result = game.ClaimPlayerColor(Guid.NewGuid(), PlayerColor.Red);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_the_same_PlayerColor_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(playerId, PlayerColor.Red);
            var result2 = game.ClaimPlayerColor(playerId, PlayerColor.Red);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Can_claim_PlayerColor_as_long_as_switching()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(playerId, PlayerColor.Blue);
            var result2 = game.ClaimPlayerColor(playerId, PlayerColor.Red);
            var result3 = game.ClaimPlayerColor(playerId, PlayerColor.Blue);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
        }

        [Test]
        public void Can_claim_previously_claimed_PlayerColor_as_long_as_switching()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(player1, PlayerColor.Blue);
            var result2 = game.ClaimPlayerColor(player2, PlayerColor.Green);
            var result3 = game.ClaimPlayerColor(player2, PlayerColor.Red);
            var result4 = game.ClaimPlayerColor(player1, PlayerColor.Red);
            var result5 = game.ClaimPlayerColor(player1, PlayerColor.Green);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsFalse(result4.Successful);
            Assert.IsTrue(result5.Successful);
        }

        [Test]
        public void Can_claim_NotChosenYet_by_multiple_players()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            var player3 = game.AddPlayer().Value;
            var player4 = game.AddPlayer().Value;

            var result1 = game.ClaimPlayerColor(player1, PlayerColor.Red);
            var result2 = game.ClaimPlayerColor(player2, PlayerColor.Blue);
            var result3 = game.ClaimPlayerColor(player3, PlayerColor.Green);
            var result4 = game.ClaimPlayerColor(player4, PlayerColor.Yellow);
            var result5 = game.ClaimPlayerColor(player1, PlayerColor.NotChosenYet);
            var result6 = game.ClaimPlayerColor(player2, PlayerColor.NotChosenYet);
            var result7 = game.ClaimPlayerColor(player3, PlayerColor.NotChosenYet);
            var result8 = game.ClaimPlayerColor(player4, PlayerColor.NotChosenYet);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
            Assert.IsTrue(result5.Successful);
            Assert.IsTrue(result6.Successful);
            Assert.IsTrue(result7.Successful);
            Assert.IsTrue(result8.Successful);
        }
    }

    [TestFixture]
    public class GameTest_SetPlayerSeat : GameTestBase
    {
        [Test]
        public void Can_set_Player_seat_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_set_Player_seat_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_set_Player_seat_for_a_non_existent_player()
        {
            var result = game.SetPlayerSeat(Guid.NewGuid(), Chair.North);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_set_the_same_Player_seat_for_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(playerId, Chair.North);
            var result2 = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Can_set_Player_seat_as_long_as_not_selecting_seat_already_in()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(playerId, Chair.North);
            var result2 = game.SetPlayerSeat(playerId, Chair.South);
            var result3 = game.SetPlayerSeat(playerId, Chair.North);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
        }

        [Test]
        public void Can_sit_at_previously_set_Player_seat_as_long_as_switching()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(player1, Chair.North);
            var result2 = game.SetPlayerSeat(player2, Chair.East);
            var result3 = game.SetPlayerSeat(player2, Chair.South);
            var result4 = game.SetPlayerSeat(player1, Chair.South);
            var result5 = game.SetPlayerSeat(player1, Chair.East);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsFalse(result4.Successful);
            Assert.IsTrue(result5.Successful);
        }

        [Test]
        public void Can_set_to_Standing_by_multiple_players()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            var player3 = game.AddPlayer().Value;
            var player4 = game.AddPlayer().Value;

            var result1 = game.SetPlayerSeat(player1, Chair.South);
            var result2 = game.SetPlayerSeat(player2, Chair.North);
            var result3 = game.SetPlayerSeat(player3, Chair.East);
            var result4 = game.SetPlayerSeat(player4, Chair.West);
            var result5 = game.SetPlayerSeat(player1, Chair.Standing);
            var result6 = game.SetPlayerSeat(player2, Chair.Standing);
            var result7 = game.SetPlayerSeat(player3, Chair.Standing);
            var result8 = game.SetPlayerSeat(player4, Chair.Standing);

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
            Assert.IsTrue(result3.Successful);
            Assert.IsTrue(result4.Successful);
            Assert.IsTrue(result5.Successful);
            Assert.IsTrue(result6.Successful);
            Assert.IsTrue(result7.Successful);
            Assert.IsTrue(result8.Successful);
        }
    }

    [TestFixture]
    public class GameTest_MarkPlayerAsReadyToStart : GameTestBase
    {
        [Test]
        public void Can_mark_Player_ready_to_start_in_ChoosePlayers_Phase()
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = GamePhase.ChoosePlayers;

            var result = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_mark_Player_ready_to_start_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_mark_Player_ready_to_start_for_a_non_existent_player()
        {
            var result = game.MarkPlayerAsReadyToStart(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_mark_the_same_Player_ready_to_start_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.MarkPlayerAsReadyToStart(playerId);
            var result2 = game.MarkPlayerAsReadyToStart(playerId);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }

        [Test]
        public void Marking_all_Players_ready_will_advance_for_2_or_more_total()
        {
            var player1 = game.AddPlayer().Value;

            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);
            var result1 = game.MarkPlayerAsReadyToStart(player1);
            Assert.AreEqual(GamePhase.ChoosePlayers, game.Phase);

            var player2 = game.AddPlayer().Value;
            var result2 = game.MarkPlayerAsReadyToStart(player2);
            WaitForBoardSetupToComplete();

            Assert.AreNotEqual(GamePhase.ChoosePlayers, game.Phase);
            Assert.AreEqual(GamePhase.PlayersPlacePeople, game.Phase);
            Assert.IsFalse(game.ClaimPlayerColor(player1, PlayerColor.NotChosenYet).Successful);
        }
    }

    [TestFixture]
    public class GameTest_RequestPlayerStats : GameTestBase
    {
        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Can_request_Player_stats_in_any_Phase(GamePhase phase)
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);
            game.Phase = phase;

            var result = game.RequestPlayerStats(player1, chair);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_a_non_existent_player()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(Guid.NewGuid(), chair);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_an_empty_Chair()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(player1, Chair.South);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_Standing_Chair()
        {
            var chair = Chair.North;
            player1 = game.AddPlayer().Value;
            game.SetPlayerSeat(player1, chair);

            var result = game.RequestPlayerStats(player1, Chair.Standing);

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    public class GameTest_RequestPlayerStats_Self : GameTestBase
    {
        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Can_request_Player_stats_in_any_Phase(GamePhase phase)
        {
            player1 = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestPlayerStats(player1);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_Player_stats_for_a_non_existent_player()
        {
            player1 = game.AddPlayer().Value;

            var result = game.RequestPlayerStats(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    public class GameTest_RequestAllPlayerStats : GameTestBase
    {
        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Can_request_all_Player_stats_in_any_Phase(GamePhase phase)
        {
            player1 = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RequestAllPlayerStats(player1);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_request_all_Player_stats_for_a_non_existent_player()
        {
            player1 = game.AddPlayer().Value;

            var result = game.RequestAllPlayerStats(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Requesting_all_Player_stats_returns_for_multiple_players()
        {
            player1 = game.AddPlayer().Value;
            game.AddPlayer();
            game.AddPlayer();
            game.AddPlayer();

            var result = game.RequestAllPlayerStats(player1);

            Assert.IsTrue(result.Successful);
            Assert.AreEqual(4, result.Value.Count());
        }
    }

    [TestFixture]
    public class GameTest_RenamePlayer : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_rename_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RenamePlayer(playerId, "James Dean");

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_rename_Player_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.RenamePlayer(playerId, "Jimmy Dean");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_any_Player_in_any_order()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;

            var result = game.RenamePlayer(player1, "Ayn Rand");

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Cannot_rename_a_non_existent_player()
        {
            var result = game.RenamePlayer(Guid.NewGuid(), "Billy Bob Roberts");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_a_removed_player()
        {
            var playerId = game.AddPlayer().Value;
            game.RemovePlayer(playerId);

            var result = game.RenamePlayer(playerId, "Robert Barber");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_the_same_player_more_than_once()
        {
            var playerId = game.AddPlayer().Value;

            var result1 = game.RenamePlayer(playerId, "Marco Polo");
            var result2 = game.RenamePlayer(playerId, "Socrates");

            Assert.IsTrue(result1.Successful);
            Assert.IsTrue(result2.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_null()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, null);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_EmptyString()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, string.Empty);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_to_all_spaces()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, "  \t  \t  \t  ");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_rename_Player_with_newline_or_carriage_return()
        {
            var playerId = game.AddPlayer().Value;

            var result = game.RenamePlayer(playerId, "\nDarth Vader\r");

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Can_rename_Player_up_to_100_characters()
        {
            var playerId = game.AddPlayer().Value;
            var longName = new string('c', 100);
            var longerName = new string('c', 101);

            var result1 = game.RenamePlayer(playerId, longName);
            var result2 = game.RenamePlayer(playerId, longerName);

            Assert.IsTrue(result1.Successful);
            Assert.IsFalse(result2.Successful);
        }
    }

    [TestFixture]
    public class GameTest_GivePlayerRandomName : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_give_random_name_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.GivePlayerRandomName(playerId);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_give_random_name_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.GivePlayerRandomName(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_give_random_name_to_a_non_existent_player()
        {
            var result = game.GivePlayerRandomName(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    public class GameTest_ChangePlayerMode : GameTestBase
    {
        // TODO: should this be an anytime setting? should this be a house rule?

        [TestCase(GamePhase.ChoosePlayers)]
        public void Can_change_PlayerMode_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ChangePlayerMode(playerId, PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_change_PlayerMode_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ChangePlayerMode(playerId, PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_change_PlayerMode_for_a_non_existent_player()
        {
            var result = game.ChangePlayerMode(Guid.NewGuid(), PlayerMode.ComputerStrategyGreedyBlock);

            Assert.IsFalse(result.Successful);
        }
    }

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

    [TestFixture]
    [Ignore]
    public class GameTest_CancelLastPlacement : GameTestBase
    {
        // TODO: this will be very house rule specific, cannot go back more than x action and cannot go back if y players have done anything

        [Test]
        public void Can_cancel_last_placement_in_PlayersPlacePeople_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.PlayersPlacePeople, game.Phase);

            var result = game.CancelLastPlacement(player1);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_cancel_last_placement_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.CancelLastPlacement(playerId);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_cancel_last_placement_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.HuntingGrounds);

            var result = game.CancelLastPlacement(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }

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

    [TestFixture]
    [Ignore]
    public class GameTest_PayForCard : GameTestBase
    {
        [Test]
        public void Can_pay_for_card_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } });

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_pay_for_card_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.PayForCard(playerId, new Dictionary<Resource, int>());

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_pay_for_card_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.PayForCard(Guid.NewGuid(), new Dictionary<Resource, int> { { Resource.Wood, 1 } });
            
            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    [Ignore]
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

    [TestFixture]
    [Ignore]
    public class GameTest_ClaimLotteryResult : GameTestBase
    {
        // TODO: potentially inject the deck of cards to make this easier to test
        [Test]
        public void Can_claim_lottery_result_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
            }
            while (card.CardImmediate != CardImmediate.Lottery);

            for (int i = 6; i >= 1; i--)
            {
                var result = game.ClaimLotteryResult(player1, i);
                if (result.Successful)
                    return;
            }
            Assert.Fail("Should have been able to claim a die.");
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.FeedPeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_claim_lottery_result_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.ClaimLotteryResult(playerId, 0);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_claim_lottery_result_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.ClaimLotteryResult(Guid.NewGuid(), 0);

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    [Ignore]
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

            var result = game.TapTool(player1, new List<Tool>{Tool.Plus1});

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

            var result = game.TapTool(playerId, new List<Tool>{Tool.Plus1});

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

            var result = game.TapTool(Guid.NewGuid(), new [] {Tool.Plus1});

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    [Ignore]
    public class GameTest_UseSpecialAction : GameTestBase
    {
        // TODO: potentially inject the deck of cards to make this easier to test
        [Test]
        public void Can_use_special_action_in_UsePeopleActions_Phase()
        {
            SetUpStandard2PlayerGame();
            for (int i = 0; i < 5; i++)
            {
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.Forest);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            while (card.CardImmediate != CardImmediate.Resources_2);
            // TODO: account for where one of the last 4/5 cards is the 2 resources card
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.UseSpecialAction(player1, SpecialAction.Take2ResourcesCard);

            Assert.IsTrue(result.Successful);
        }

        [Test]
        public void Can_use_special_action_in_FeedPeople_Phase()
        {
            var player1 = game.AddPlayer().Value;
            var player2 = game.AddPlayer().Value;
            game.RequestStartPlayer(player1);
            game.MarkPlayerAsReadyToStart(player1);
            game.MarkPlayerAsReadyToStart(player2);
            WaitForBoardSetupToComplete();
            for (int i = 0; i < 5; i++)
            {
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.Field);
                game.PlacePeople(player1, 4, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Field);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            Card card;
            do
            {
                game.PlacePeople(player1, 5, BoardSpace.Forest);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
                game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
                game.PlacePeople(player1, 1, BoardSpace.CivilizationCardSlot1);
                game.PlacePeople(player1, 4, BoardSpace.Forest);
                game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
                game.UseActionOfPeople(player1, BoardSpace.CivilizationCardSlot1);
                card = game.PayForCard(player1, new Dictionary<Resource, int> { { Resource.Wood, 1 } }).Value;
                game.UseActionOfPeople(player1, BoardSpace.Forest);
                game.FeedPeople(player1);
                game.FeedPeople(player2);
            }
            while (card.CardImmediate != CardImmediate.Resources_2);
            // TODO: account for where one of the last 4/5 cards is the 2 resources card
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);

            var result = game.UseSpecialAction(player1, SpecialAction.Take2ResourcesCard);

            Assert.IsTrue(result.Successful);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_use_special_action_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.UseSpecialAction(playerId, SpecialAction.Take2ResourcesCard);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_use_special_action_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 1, BoardSpace.BuildingTileSlot1);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player1, 4, BoardSpace.Forest);
            Assert.AreEqual(GamePhase.UsePeopleActions, game.Phase);

            var result = game.UseSpecialAction(Guid.NewGuid(), SpecialAction.Take2ResourcesCard);

            Assert.IsFalse(result.Successful);
        }
    }

    [TestFixture]
    public class GameTest_FeedPeople : GameTestBase
    {
        [Test]
        public void Can_feed_people_in_FeedPeople_Phase()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);
            var preFeedFood = game.RequestPlayerStats(player1).Value.Food;

            var result = game.FeedPeople(player1);

            Assert.IsTrue(result.Successful);
            var postFeedFood = game.RequestPlayerStats(player1).Value.Food;
            Assert.AreEqual(preFeedFood, postFeedFood + 5);
        }

        [TestCase(GamePhase.ChoosePlayers)]
        [TestCase(GamePhase.SetUpBoard)]
        [TestCase(GamePhase.PlayersPlacePeople)]
        [TestCase(GamePhase.UsePeopleActions)]
        [TestCase(GamePhase.CheckIfEndGame)]
        [TestCase(GamePhase.NewRoundPrep)]
        [TestCase(GamePhase.FinalScoring)]
        public void Cannot_feed_people_in_Phase(GamePhase phase)
        {
            var playerId = game.AddPlayer().Value;
            game.Phase = phase;

            var result = game.FeedPeople(player1);

            Assert.IsFalse(result.Successful);
        }

        [Test]
        public void Cannot_feed_people_for_a_non_existent_player()
        {
            SetUpStandard2PlayerGame();
            game.PlacePeople(player1, 5, BoardSpace.HuntingGrounds);
            game.PlacePeople(player2, 5, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player1, BoardSpace.HuntingGrounds);
            game.UseActionOfPeople(player2, BoardSpace.HuntingGrounds);
            Assert.AreEqual(GamePhase.FeedPeople, game.Phase);

            var result = game.FeedPeople(Guid.NewGuid());

            Assert.IsFalse(result.Successful);
        }
    }

    public class UltimatePlayerBoardFactory : IPlayerBoardFactory
    {
        public PlayerBoard CreateNew()
        {
            var playerBoard = new PlayerBoard
            {
                Food = int.MaxValue,
                FoodTrack = 10,
                PeopleToPlace = 10,
                TotalPeople = 10,
            };

            playerBoard.Resources[Resource.Wood] = 999;
            playerBoard.Resources[Resource.Brick] = 999;
            playerBoard.Resources[Resource.Stone] = 999;
            playerBoard.Resources[Resource.Gold] = 999;

            playerBoard.Tools[0] = Tool.Plus4;
            playerBoard.Tools[1] = Tool.Plus4;
            playerBoard.Tools[2] = Tool.Plus4;

            return playerBoard;
        }
    }
}
