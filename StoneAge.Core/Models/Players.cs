using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.Core.Models
{
    [System.Diagnostics.DebuggerDisplay("{Name} ({Mode}) {Color} {Chair}")]
    public class Player
    {
        public const int MAX_NAME_LENGTH = 100;

        public Guid Id;
        public string Name;
        public PlayerMode Mode;
        public PlayerColor Color;
        public Chair Chair;
        public PlayerBoard PlayerBoard;
        public bool WantsToBeFirstPlayer;
        public bool ReadyToStart;

        public Player()
        {
            Id = Guid.NewGuid();
            Name = DefaultPlayerNames[_random.Next(DefaultPlayerNames.Length)];
            Color = PlayerColor.NotChosenYet;
        }

        private static readonly Random _random = new Random();
        public static string[] DefaultPlayerNames = new[]
        {
            "Michael",
            "Carol",
            "Scott",
            "Olivia",
            "Griffin",
            "Benjamin",
            "Tiffany",
            "Brooklynn",
            "Shane",
            "Matt",
            "Becky",
            "Oliver",
            "Dave",
            "Mel",
            "Abby",
            "Ash",
        };
    }

    public class PlayerBoard
    {
        public int Food;
        public readonly IDictionary<Resource, int> Resources = new Dictionary<Resource, int>();
        public int TotalPeople;
        public int PeopleToPlace;
        public readonly Tool[] Tools = new Tool[3];
        public int FoodTrack;
        public int Score;
        public bool DroppedOut;

        public PlayerBoard()
        {
            TotalPeople = STARTING_PEOPLE_COUNT;
            PeopleToPlace = TotalPeople;

            Food = STARTING_FOOD_COUNT;

            Resources.Add(Resource.Wood, STARTING_RESOURCE_COUNT);
            Resources.Add(Resource.Brick, STARTING_RESOURCE_COUNT);
            Resources.Add(Resource.Stone, STARTING_RESOURCE_COUNT);
            Resources.Add(Resource.Gold, STARTING_RESOURCE_COUNT);

            Tools[0] = Tool.None;
            Tools[1] = Tool.None;
            Tools[2] = Tool.None;

            FoodTrack = STARTING_FOOD_TRACK;
            Score = STARTING_SCORE;
        }

        public bool HasAvailablePeopleToPlace(int quantity)
        {
            return PeopleToPlace >= quantity;
        }

        public void SetPeopleAsPlaced(int quantity)
        {
            PeopleToPlace -= quantity;
        }

        private const int STARTING_PEOPLE_COUNT = 5;
        private const int STARTING_FOOD_COUNT = 12;
        private const int STARTING_RESOURCE_COUNT = 0;
        private const int STARTING_FOOD_TRACK = 0;
        private const int STARTING_SCORE = 0;
        private const int MAX_PEOPLE_COUNT = 10;
        private const int MAX_FOOD_TRACK = 10;
    }

    public enum PlayerColor
    {
        NotChosenYet,
        Green,
        Red,
        Blue,
        Yellow,
    }

    public enum PlayerMode
    {
        HumanLocal,
        HumanInternet,
        ComputerStrategyRandom,
        ComputerStrategyPeople,
        ComputerStrategyFarm,
        ComputerStrategyTool,
        ComputerStrategyCard,
        ComputerStrategyHuts,
        ComputerStrategyGreedyBlock,
        ComputerEasy,// not sure what this means
        ComputerHard,// not sure what this means
        ComputerUnknown,// chooses a strategy at random
    }

    public enum Chair
    {
        Standing,
        North,
        East,
        South,
        West,
    }
}
