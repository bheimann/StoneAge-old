using System;
using System.Threading;
using NUnit.Framework;
using StoneAge.Core.Models;
using StoneAge.Core.Models.Players;
using StoneAge.Core.Models.Tools;

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

    public class UltimatePlayerBoardFactory : IPlayerBoardFactory
    {
        public PlayerBoard CreateNew()
        {
            var playerBoard = new PlayerBoard
            {
                Food = 1980,
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
