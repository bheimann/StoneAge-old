using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core
{
    public class GameBoard
    {
        public const int TOTAL_WOOD = 20;
        public const int TOTAL_BRICK = 16;
        public const int TOTAL_STONE = 12;
        public const int TOTAL_GOLD = 10;

        public int Wood;
        public int Brick;
        public int Stone;
        public int Gold;

        public List<PlayerBoard> Players = new List<PlayerBoard>();

        public PlayerBoard Current
        {
            get
            {
                return Players[_current];
            }
        }

        public GameBoard()
            : this(PlayerColor.Blue, PlayerColor.Green, PlayerColor.Red, PlayerColor.Yellow)
        {
        }

        public GameBoard(params PlayerColor[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                var player = new PlayerBoard(args[i])
                {
                    Name = _defaultPlayers[i],
                };
                Players.Add(player);
            }

            Wood = TOTAL_WOOD;
            Brick = TOTAL_BRICK;
            Stone = TOTAL_STONE;
            Gold = TOTAL_GOLD;
        }

        public void Next()
        {
            ++_current;
            if (_current >= Players.Count())
                _current = 0;
        }

        private string[] _defaultPlayers = new [] { "Harry", "Mary", "Frank", "Jane" };
        private int _current = 0;
    }

    public class PlayerBoard
    {
        public readonly PlayerColor Color;
        public int Food;
        public readonly IDictionary<Resource, int> Resources = new Dictionary<Resource, int>();
        public int People;
        public readonly Tool[] Tools = new Tool[3];
        public string Name;
        public int FoodTrack;
        public int Score;

        public PlayerBoard(PlayerColor color)
        {
            Color = color;

            People = STARTING_PEOPLE_COUNT;

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

        private const int STARTING_PEOPLE_COUNT = 5;
        private const int STARTING_FOOD_COUNT = 12;
        private const int STARTING_RESOURCE_COUNT = 0;
        private const int STARTING_FOOD_TRACK = 0;
        private const int STARTING_SCORE = 0;
        private const int MAX_PEOPLE_COUNT = 10;
        private const int MAX_FOOD_TRACK = 10;
    }

    public enum Tool
    {
        None,
        Plus1,
        Plus2,
        Plus3,
        Plus4,
    }

    public enum PlayerColor
    {
        Green,
        Red,
        Blue,
        Yellow,
    }

    public enum BoardSpace
    {
        HuntingGrounds,
        Forest,
        ClayPit,
        Quarry,
        River,
        ToolMaker,
        Hut,
        Field,
        CivilizationCardSlot1,
        CivilizationCardSlot2,
        CivilizationCardSlot3,
        CivilizationCardSlot4,
        BuildingTileSlot1,
        BuildingTileSlot2,
        BuildingTileSlot3,
        BuildingTileSlot4,
    }

    public enum Resource
    {
        Wood,
        Brick,
        Stone,
        Gold,
    }
}
