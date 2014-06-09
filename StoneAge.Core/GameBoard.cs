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
        public Dictionary<BoardSpace, Space> Spaces = new Dictionary<BoardSpace, Space>();

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

            AddedDefaultSpaces();
        }

        private void AddedDefaultSpaces()
        {
            var boardSpaces = System.Enum.GetValues(typeof(BoardSpace)).Cast<BoardSpace>().Where(bs => bs != BoardSpace.HuntingGrounds);

            foreach (var boardSpace in boardSpaces)
            {
                Spaces.Add(boardSpace, new Space());
            }
        }

        public void Next()
        {
            ++_current;
            if (_current >= Players.Count())
                _current = 0;

            foreach (Space space in Spaces.Values)
            {
                if (space.HeldBy == null)
                {
                    space.HeldBy = space.ThinkingOf;
                    space.ThinkingOf = null;
                }
            }
        }


        public void TryToOccupySpace(BoardSpace Space)
        {
            if (Spaces[Space].HeldBy.HasValue)
                return;

            if (Spaces[Space].ThinkingOf == null)
                Spaces[Space].ThinkingOf = Current.Color;
            else
                Spaces[Space].ThinkingOf = null;
        }

        public PlayerColor? ColorOFSpace(BoardSpace Space)
        {
            if (Spaces[Space].HeldBy.HasValue)
            {
                return Spaces[Space].HeldBy;
            }
            return Spaces[Space].ThinkingOf;
        }

        private string[] _defaultPlayers = new [] { "Harry", "Mary", "Frank", "Jane" };
        private int _current = 0;
    }

    public class Space
    {
        public PlayerColor? HeldBy;
        public PlayerColor? ThinkingOf;
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

    public struct Tool
    {
        public static Tool None  { get { return new Tool(0); } }
        public static Tool Plus1 { get { return new Tool(1); } }
        public static Tool Plus2 { get { return new Tool(2); } }
        public static Tool Plus3 { get { return new Tool(3); } }
        public static Tool Plus4 { get { return new Tool(4); } }

        public bool Used;
        public readonly int Value;

        private Tool(int value)
	    {
            Value = value;
            Used = false;
	    }
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
        Forest1,
        Forest2,
        Forest3,
        Forest4,
        Forest5,
        Forest6,
        Forest7,
        ClayPit1,
        ClayPit2,
        ClayPit3,
        ClayPit4,
        ClayPit5,
        ClayPit6,
        ClayPit7,
        Quarry1,
        Quarry2,
        Quarry3,
        Quarry4,
        Quarry5,
        Quarry6,
        Quarry7,
        River1,
        River2,
        River3,
        River4,
        River5,
        River6,
        River7,
        ToolMaker1,
        Hut1,
        Hut2,
        Field1,
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
