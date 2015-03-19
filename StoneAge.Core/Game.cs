using StoneAge.Core.Models;
using StoneAge.Core.Models.BoardSpaces;
using StoneAge.Core.Models.Cards;
using StoneAge.Core.Models.Players;
using StoneAge.Core.Models.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoneAge.Core
{
    //Flow Diagram:

    //Get # players
    //  Human/Computer
    //  Online/Local
    //Choose Colors & turn order

    //Set up board
    //  Shuffle cards
    //  Shuffle hut tiles
    //  Wood, brick, stone, & gold placed
    //  Place tools
    //  Place stacks of huts = players

    //Set up players
    //  Give 5 people, 12 food, 0 rest (could have house rules)
    //  Pick starting player

    //---------- Round ----------
    //Prep new round
    //  Expose up to top 4 cards
    //  Expose top hut stacks
    //  Place chieftain
    //Players place people
    //  Until there are no more to place
    //  Cannot add more to same place (except food)
    //  Note player space limits
    //  Resource limits could be house rules
    //Use actions of people
    //  One player at a time
    //  Each player removes all people
    //  Lottery notifications
    //Feed People
    //  Potential for -10 points
    //  1 food/person, 1 resource/person, subtract food track
    //Check for end game?
    //  Are there less than 4 cards remaining?
    //  Is a hut tile stack empty?

    //Final Scoring

    [System.Diagnostics.DebuggerDisplay("{RoundNumber} {Phase}")]
    public class Game
    {
        // have a mechanism for giving status notifications
        public bool IsThinking;

        public const int MAX_PLAYER_COUNT = 4;
        public GamePhase Phase = GamePhase.ChoosePlayers;
        public int RoundNumber = 1; // TODO: make sure the round is incremented :)

        public Game()
            : this(new StandardPlayerBoardFactory())
        {
        }

        public Game(IPlayerBoardFactory playerBoardFactory)
        {
            _playerBoardFactory = playerBoardFactory;

            _dicePouch = new DicePouch();
        }

        public GameResponse<Guid> AddPlayer()
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse<Guid>.Fail();

            if (_players.Count() == MAX_PLAYER_COUNT)
                return GameResponse<Guid>.Fail();

            var player = new Player();
            _players.Add(player);

            return GameResponse<Guid>.Pass(player.Id);
        }

        public GameResponse RemovePlayer(Guid playerId)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var playerToRemove = _players.SingleOrDefault(p => p.Id == playerId);

            if(!_players.Remove(playerToRemove))
                return GameResponse.Fail();

            return GameResponse.Pass();
        }

        // puts player in drawing for 1st player
        public GameResponse RequestStartPlayer(Guid playerId)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if(player == null)
                return GameResponse.Fail();

            if (player.WantsToBeFirstPlayer)
                return GameResponse.Fail();

            player.WantsToBeFirstPlayer = true;

            return GameResponse.Pass();
        }

        // removes player from 1st player drawing
        public GameResponse DeclineStartPlayer(Guid playerId)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            if (!player.WantsToBeFirstPlayer)
                return GameResponse.Fail();

            player.WantsToBeFirstPlayer = false;

            return GameResponse.Pass();
        }

        // should this just be random? Maybe make house rules?
        // which colors? Any order for choice?
        public GameResponse ClaimPlayerColor(Guid playerId, PlayerColor color)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();
            if (player.Color == color)
                return GameResponse.Fail();

            if (color != PlayerColor.NotChosenYet && _players.Any(p => p.Color == color))
                return GameResponse.Fail();

            player.Color = color;

            return GameResponse.Pass();
        }

        // should this just be random? Maybe make house rules?
        public GameResponse SetPlayerSeat(Guid playerId, Chair chair)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();
            if (player.Chair == chair)
                return GameResponse.Fail();

            if (chair != Chair.Standing && _players.Any(p => p.Chair == chair))
                return GameResponse.Fail();

            player.Chair = chair;

            return GameResponse.Pass();
        }

        // moves to next phase once all are ready
        public GameResponse MarkPlayerAsReadyToStart(Guid playerId)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();
            if (player.ReadyToStart)
                return GameResponse.Fail();

            player.ReadyToStart = true;

            if (_players.All(p => p.ReadyToStart) && _players.Count() > 1)
            {
                IsThinking = true;
                var task = new Task<bool>(InitialBoardSetup);
                task.Start();
            }

            return GameResponse.Pass();
        }

        private bool InitialBoardSetup()
        {
            Phase = GamePhase.SetUpBoard;
            Board = new GameBoard();
            TurnOrder = new TurnOrder();

            AssignPlayerSeats();
            AssignPlayerColors();
            PassOutPlayerBoards();

            PrepareNewRound();
            ChooseStartPlayer();

            // TODO: "how to solve call back and setting up the board"
            IsThinking = false;
            Phase = GamePhase.PlayersPlacePeople;
            return true;
        }

        private void AssignPlayerSeats()
        {
            var allChairs = new[] { Chair.North, Chair.East, Chair.South, Chair.West };
            foreach (var player in _players)
            {
                var alreadyChoseChair = player.Chair != Chair.Standing;
                if (alreadyChoseChair)
                    continue;

                var takenChairs = _players.Select(p => p.Chair);
                var freeSeats = allChairs.Where(c => !takenChairs.Contains(c));
                var chair = freeSeats.ChooseAtRandom();
                player.Chair = chair;
            }

            foreach (var chair in allChairs)
                AddTakenChairsToQueue(chair);
        }

        private void AddTakenChairsToQueue(Chair chair)
        {
            var playerInChair = _players.SingleOrDefault(p => p.Chair == chair);
            if (playerInChair != null)
                TurnOrder.AddToEnd(playerInChair);
        }

        private void AssignPlayerColors()
        {
            var allColors = new[] { PlayerColor.Blue, PlayerColor.Green, PlayerColor.Red, PlayerColor.Yellow };
            foreach (var player in _players)
            {
                var alreadyChoseColor = player.Color != PlayerColor.NotChosenYet;
                if (alreadyChoseColor)
                    continue;

                var takenColors = _players.Select(p => p.Color);
                var freeColors = allColors.Where(c => !takenColors.Contains(c));
                var color = freeColors.ChooseAtRandom();
                player.Color = color;
            }
        }

        private void ChooseStartPlayer()
        {
            var potentialStartPlayers = _players
                .Where(p => p.WantsToBeFirstPlayer)
                .ToList();

            var player = potentialStartPlayers.Any() ?
                potentialStartPlayers.ChooseAtRandom() :
                _players.ChooseAtRandom();

            TurnOrder.SetCheiftan(player);
        }

        private void PassOutPlayerBoards()
        {
            foreach (var player in _players)
                player.PlayerBoard = _playerBoardFactory.CreateNew();
        }

        private bool PrepareNewRound()
        {
            var gameOverSignafied = false;

            var cardsForSlots = new Queue<Card>();
            if (Board.CardSlot1 != null)
                cardsForSlots.Enqueue(Board.CardSlot1);
            if (Board.CardSlot2 != null)
                cardsForSlots.Enqueue(Board.CardSlot2);
            if (Board.CardSlot3 != null)
                cardsForSlots.Enqueue(Board.CardSlot3);
            if (Board.CardSlot4 != null)
                cardsForSlots.Enqueue(Board.CardSlot4);

            for (int i = cardsForSlots.Count(); i < 4; i++)
            {
                if (!Board.CardDeck.Any())
                {
                    gameOverSignafied = true;
                    break;
                }
                cardsForSlots.Enqueue(Board.CardDeck.Pop());
            }

            if(!gameOverSignafied)
            {
                Board.CardSlot1 = cardsForSlots.Dequeue();
                Board.CardSlot2 = cardsForSlots.Dequeue();
                Board.CardSlot3 = cardsForSlots.Dequeue();
                Board.CardSlot4 = cardsForSlots.Dequeue();
            }

            Board.HutStack1.FlipTopTile();
            Board.HutStack2.FlipTopTile();
            Board.HutStack3.FlipTopTile();
            Board.HutStack4.FlipTopTile();

            if (Board.HutStack1.IsEmpty ||
                Board.HutStack2.IsEmpty ||
                Board.HutStack3.IsEmpty ||
                Board.HutStack4.IsEmpty)
            {
                return true;
            }

            TurnOrder.NextCheiftan();

            // TODO: Check once complete as I think this isn't needed
            foreach (var player in _players)
            {
                player.PlayerBoard.PeopleToPlace = player.PlayerBoard.TotalPeople;
            }

            return gameOverSignafied;
        }

        public GameResponse RenamePlayer(Guid playerId, string newName)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            if (string.IsNullOrWhiteSpace(newName))
                return GameResponse.Fail();

            if (newName.Contains('\n') || newName.Contains('\r'))
                return GameResponse.Fail();

            if (newName.Length > Player.MAX_NAME_LENGTH)
                return GameResponse.Fail();

            var playerToRename = _players.SingleOrDefault(p => p.Id == playerId);

            if(playerToRename == null)
                return GameResponse.Fail();

            playerToRename.Name = newName;

            return GameResponse.Pass();
        }

        public GameResponse GivePlayerRandomName(Guid playerId)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            var newName = Player.DefaultPlayerNames.ChooseAtRandom();
            player.Name = newName;

            return GameResponse.Pass();
        }

        public GameResponse ChangePlayerMode(Guid playerId, PlayerMode mode)
        {
            if (Phase != GamePhase.ChoosePlayers)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            player.Mode = mode;

            return GameResponse.Pass();
        }

        public GameResponse PlacePeople(Guid playerId, int quantity, BoardSpace boardSpace)
        {
            if (Phase != GamePhase.PlayersPlacePeople)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            var space = Board.Spaces.SingleOrDefault(s => s.BoardSpace == boardSpace);
            if (space.QuantityIsInvalidForSpace(quantity))
                return GameResponse.Fail();

            if(space.PlayerPreviouslyPlaced(player))
                return GameResponse.Fail();

            if(space.NotAvailable(quantity))
                return GameResponse.Fail();

            if(space.HasTooManyUniquePlayers())
                return GameResponse.Fail();

            if(!player.PlayerBoard.HasAvailablePeopleToPlace(quantity))
                return GameResponse.Fail();

            player.PlayerBoard.SetPeopleAsPlaced(quantity);
            space.Place(player, quantity);

            if (_players.Sum(p => p.PlayerBoard.PeopleToPlace) == 0)
                Phase = GamePhase.UsePeopleActions;

            return GameResponse.Pass();
        }

        public GameResponse CancelLastPlacement(Guid playerId)
        {
            if (Phase != GamePhase.PlayersPlacePeople)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            // should this be allowed? probably will be a house rule including things like how many times
            throw new NotImplementedException();
        }

        public GameResponse<DiceResult> UseActionOfPeople(Guid playerId, BoardSpace boardSpace)
        {
            if (Phase != GamePhase.UsePeopleActions)
                return GameResponse<DiceResult>.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse<DiceResult>.Fail();

            var space = Board.Spaces.SingleOrDefault(s => s.BoardSpace == boardSpace);
            if(!space.PlayerPreviouslyPlaced(player))
                return GameResponse<DiceResult>.Fail();

            var diceResult = UseAction(player, space);

            // TODO: start
            //if(_players.Sum(p => p.PlayerBoard.Re)
            //Board.Spaces.Sum(s => s.QuantityPlaced)


            return GameResponse<DiceResult>.Pass(diceResult);
        }

        private DiceResult UseAction(Player player, Space space)
        {
            var result = new DiceResult(new int [0]);
            switch (space.BoardSpace)
            {
                case BoardSpace.HuntingGrounds:
                    result = _dicePouch.Roll(space.QuantityPlaced(player));
                    var diceSum = result.Sum();
                    var numberOfFood = diceSum / 3;
                    player.PlayerBoard.Food += numberOfFood;
                    break;
                case BoardSpace.Forest:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.ClayPit:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.Quarry:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.River:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.ToolMaker:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.Hut:
                    ++player.PlayerBoard.PeopleToPlace;
                    ++player.PlayerBoard.TotalPeople;
                    break;
                case BoardSpace.Field:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.CivilizationCardSlot1:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.CivilizationCardSlot2:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.CivilizationCardSlot3:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.CivilizationCardSlot4:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.BuildingTileSlot1:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.BuildingTileSlot2:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.BuildingTileSlot3:
                    throw new NotImplementedException();
                    break;
                case BoardSpace.BuildingTileSlot4:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            space.ReturnToPlayer(player);
            return result;
        }

        public GameResponse<Card> PayForCard(Guid playerId, IDictionary<Resource, int> resources)
        {
            if (Phase != GamePhase.UsePeopleActions)
                return GameResponse<Card>.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse<Card>.Fail();

            throw new NotImplementedException();
        }

        public GameResponse<int> PayForHutTile(Guid playerId, IDictionary<Resource, int> resources)
        {
            if (Phase != GamePhase.UsePeopleActions)
                return GameResponse<int>.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse<int>.Fail();

            throw new NotImplementedException();
        }

        public GameResponse ClaimLotteryResult(Guid playerId, int rollValue)
        {
            if (Phase != GamePhase.UsePeopleActions)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            throw new NotImplementedException();
        }

        public GameResponse TapTool(Guid playerId, IList<Tool> tools)
        {
            if (Phase != GamePhase.UsePeopleActions)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            throw new NotImplementedException();
        }

        public GameResponse UseSpecialAction(Guid playerId, SpecialAction action)
        {
            if (Phase != GamePhase.UsePeopleActions && Phase != GamePhase.FeedPeople)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            // any time, e.g. 2 resource card
            throw new NotImplementedException();
        }

        public GameResponse FeedPeople(Guid playerId)
        {
            return FeedPeople(playerId, new Dictionary<Resource, int>());
        }

        public GameResponse FeedPeople(Guid playerId, IDictionary<Resource, int> otherResourcesToFeedWith)
        {
            if (Phase != GamePhase.FeedPeople)
                return GameResponse.Fail();

            var player = _players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
                return GameResponse.Fail();

            throw new NotImplementedException();
        }

        public GameResponse<int> CalculateCurrentScore(Guid playerId)
        {
            // should be any time
            throw new NotImplementedException();
        }

        public GameBoard Board;
        private readonly IList<Player> _players = new List<Player>();

        //private int _current = 0;
        //public Player Current
        //{
        //    get
        //    {
        //        return _players[_current];
        //    }
        //}

        private TurnOrder TurnOrder;
        private DicePouch _dicePouch;
        private IPlayerBoardFactory _playerBoardFactory;
    }

    public enum GamePhase
    {
        ChoosePlayers,
        SetUpBoard,
        PlayersPlacePeople,
        UsePeopleActions,
        FeedPeople,
        CheckIfEndGame,
        NewRoundPrep,
        FinalScoring,
    }

    public class TurnOrder
    {
        private readonly Queue<Player> _playerQueue = new Queue<Player>();

        private Chair _cheiftanChair = Chair.Standing;

        public void AddToEnd(Player player)
        {
            if (_playerQueue.Contains(player))
                throw new Exception("Cannot add the same player more than once");

            _playerQueue.Enqueue(player);
        }

        public Player Next()
        {
            var moveToBack = _playerQueue.Dequeue();
            _playerQueue.Enqueue(moveToBack);

            return Current();
        }

        public Player Current()
        {
            return _playerQueue.Peek();
        }

        public void SetCheiftan(Player player)
        {
            _cheiftanChair = player.Chair;
            while (player.Chair != Next().Chair)
                ;
        }

        public void NextCheiftan()
        {
            if (_cheiftanChair == Chair.Standing)
                return;
            while (_cheiftanChair != Next().Chair)
                ;
            _cheiftanChair = Next().Chair;
        }
    }
}
