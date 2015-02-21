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

            AddDefaultSpaces();
        }

        private void AddDefaultSpaces()
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

        public PlayerColor? ColorOfSpace(BoardSpace Space)
        {
            if (Spaces[Space].HeldBy.HasValue)
            {
                return Spaces[Space].HeldBy;
            }
            return Spaces[Space].ThinkingOf;
        }

        private string[] _defaultPlayers = new[] { "Harry", "Mary", "Frank", "Jane" };
        private int _current = 0;
    }

    public enum GamePhase
    {
        PlacePeople,
        UseActions,
        FeedPeople,
        NewRound,
        FinalScoring,
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
        public bool DroppedOut;

        public PlayerBoard(string name, PlayerColor color)
            : this(color)
        {
            Name = name;
        }

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
        public static Tool None { get { return new Tool(0); } }
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
        Wood = 3,
        Brick = 4,
        Stone = 5,
        Gold = 6,
    }

    public class BuildingTile
    {
        // Should be 28 total
        public static readonly List<BuildingTile> All = new List<BuildingTile>();

        public static BuildingTile WWB1 = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Brick);
        public static BuildingTile WWS1 = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Stone);
        public static BuildingTile WWG1 = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Gold);
        public static BuildingTile BBS1 = new BuildingTile(Resource.Brick, Resource.Brick, Resource.Stone);
        public static BuildingTile BBG1 = new BuildingTile(Resource.Brick, Resource.Brick, Resource.Gold);
        public static BuildingTile SSG1 = new BuildingTile(Resource.Stone, Resource.Stone, Resource.Gold);
        public static BuildingTile WBB1 = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Brick);
        public static BuildingTile WSS1 = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Stone);
        public static BuildingTile BSS1 = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Stone);

        public static BuildingTile WBS1 = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Stone);
        public static BuildingTile WBS2 = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Stone);
        public static BuildingTile WSG1 = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Gold);
        public static BuildingTile WSG2 = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Gold);
        public static BuildingTile WBG1 = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Gold);
        public static BuildingTile WBG2 = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Gold);
        public static BuildingTile BSG1 = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Gold);
        public static BuildingTile BSG2 = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Gold);

        public static BuildingTile C411 = new BuildingTile(4, 1);
        public static BuildingTile C421 = new BuildingTile(4, 2);
        public static BuildingTile C431 = new BuildingTile(4, 3);
        public static BuildingTile C441 = new BuildingTile(4, 4);

        public static BuildingTile C511 = new BuildingTile(5, 1);
        public static BuildingTile C521 = new BuildingTile(5, 2);
        public static BuildingTile C531 = new BuildingTile(5, 3);
        public static BuildingTile C541 = new BuildingTile(5, 4);

        public static BuildingTile C171 = new BuildingTile(1, 7, 1);
        public static BuildingTile C172 = new BuildingTile(1, 7, 1);
        public static BuildingTile C173 = new BuildingTile(1, 7, 1);

        public readonly Resource? Resource1;
        public readonly Resource? Resource2;
        public readonly Resource? Resource3;

        public int? NumberOfUniqueResources;
        public int? MinQuantity;
        public int? MaxQuantity;

        public int? MinValue
        {
            get
            {
                if (Resource1.HasValue && Resource2.HasValue && Resource3.HasValue)
                    return (int)Resource1 + (int)Resource2 + (int)Resource3;

                if (NumberOfUniqueResources == 1)
                    return (int)Resource.Wood * MinQuantity.Value;
                if (NumberOfUniqueResources == 2)
                    return (int)Resource.Wood * MinQuantity.Value + 1;
                if (NumberOfUniqueResources == 3)
                    return (int)Resource.Wood * MinQuantity.Value + 3;
                if (NumberOfUniqueResources == 4)
                    return (int)Resource.Wood * MinQuantity.Value + 6;

                return 0;
            }
        }

        public int? MaxValue
        {
            get
            {
                if (Resource1.HasValue && Resource2.HasValue && Resource3.HasValue)
                    return (int)Resource1 + (int)Resource2 + (int)Resource3;

                if (NumberOfUniqueResources == 1)
                    return (int)Resource.Gold * MaxQuantity.Value;
                if (NumberOfUniqueResources == 2)
                    return (int)Resource.Gold * MaxQuantity.Value - 1;
                if (NumberOfUniqueResources == 3)
                    return (int)Resource.Gold * MaxQuantity.Value - 3;
                if (NumberOfUniqueResources == 4)
                    return (int)Resource.Gold * MaxQuantity.Value - 6;

                return 0;
            }
        }

        private BuildingTile(Resource resource1, Resource resource2, Resource resource3)
        {
            Resource1 = resource1;
            Resource2 = resource2;
            Resource3 = resource3;

            All.Add(this);
        }

        private BuildingTile(int minQuantity, int maxQuantity, int numberOfUniqueResources)
        {
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            NumberOfUniqueResources = numberOfUniqueResources;

            All.Add(this);
        }

        private BuildingTile(int quantity, int numberOfUniqueResources)
            : this(quantity, quantity, numberOfUniqueResources)
        {
        }
    }

    public enum CardImmediate
    {
        Brick_1,
        Stone_1,
        Gold_1,
        Stone_2,
        Lottery,
        FarmTrack,
        Tool_Permanent,
        Roll_Wood,
        Roll_Stone,
        Roll_Gold,
        Food_1,
        Food_2,
        Food_3,
        Food_4,
        Food_5,
        Food_7,
        Points_3,
        DrawCard,
        Resources_2,
        Tool_2,
        Tool_3,
        Tool_4,
    }

    public enum CardFinalScoring
    {
        // Green
        Transport,
        Time,
        Pottery,
        Art,
        Healing,
        Music,
        Writing,
        Weaving,
        // Brown
        HutBuilder_1,
        HutBuilder_2,
        HutBuilder_3,
        Farmer_1,
        Farmer_2,
        Shaman_1,
        Shaman_2,
        ToolMaker_1,
        ToolMaker_2,
    }

    [System.Diagnostics.DebuggerDisplay("{CardFinalScoring} {CardImmediate}")]
    public class Card
    {
        // Should be 36 total
        public static readonly IList<Card> All = new List<Card>();

        public static Card GTr1 = new Card(CardFinalScoring.Transport, CardImmediate.Stone_2);
        public static Card GTr2 = new Card(CardFinalScoring.Transport, CardImmediate.Lottery);
        public static Card GTi1 = new Card(CardFinalScoring.Time, CardImmediate.FarmTrack);
        public static Card GTi2 = new Card(CardFinalScoring.Time, CardImmediate.Lottery);
        public static Card GPo1 = new Card(CardFinalScoring.Pottery, CardImmediate.Food_7);
        public static Card GPo2 = new Card(CardFinalScoring.Pottery, CardImmediate.Lottery);
        public static Card GAr1 = new Card(CardFinalScoring.Art, CardImmediate.Tool_Permanent);
        public static Card GAr2 = new Card(CardFinalScoring.Art, CardImmediate.Lottery);
        public static Card GHe1 = new Card(CardFinalScoring.Healing, CardImmediate.Food_5);
        public static Card GHe2 = new Card(CardFinalScoring.Healing, CardImmediate.Resources_2);
        public static Card GMu1 = new Card(CardFinalScoring.Music, CardImmediate.Points_3);
        public static Card GMu2 = new Card(CardFinalScoring.Music, CardImmediate.Points_3);
        public static Card GWr1 = new Card(CardFinalScoring.Writing, CardImmediate.DrawCard);
        public static Card GWr2 = new Card(CardFinalScoring.Writing, CardImmediate.Lottery);
        public static Card GWe1 = new Card(CardFinalScoring.Weaving, CardImmediate.Food_1);
        public static Card GWe2 = new Card(CardFinalScoring.Weaving, CardImmediate.Food_3);

        public static Card BH11 = new Card(CardFinalScoring.HutBuilder_1, CardImmediate.Food_4);
        public static Card BH12 = new Card(CardFinalScoring.HutBuilder_1, CardImmediate.Lottery);
        public static Card BH21 = new Card(CardFinalScoring.HutBuilder_2, CardImmediate.Food_2);
        public static Card BH22 = new Card(CardFinalScoring.HutBuilder_2, CardImmediate.Lottery);
        public static Card BH31 = new Card(CardFinalScoring.HutBuilder_3, CardImmediate.Points_3);

        public static Card BF11 = new Card(CardFinalScoring.Farmer_1, CardImmediate.Stone_1);
        public static Card BF12 = new Card(CardFinalScoring.Farmer_1, CardImmediate.FarmTrack);
        public static Card BF13 = new Card(CardFinalScoring.Farmer_1, CardImmediate.Lottery);
        public static Card BF21 = new Card(CardFinalScoring.Farmer_2, CardImmediate.Food_3);
        public static Card BF22 = new Card(CardFinalScoring.Farmer_2, CardImmediate.Lottery);

        public static Card BS11 = new Card(CardFinalScoring.Shaman_1, CardImmediate.Stone_1);
        public static Card BS12 = new Card(CardFinalScoring.Shaman_1, CardImmediate.Gold_1);
        public static Card BS13 = new Card(CardFinalScoring.Shaman_1, CardImmediate.Roll_Stone);
        public static Card BS21 = new Card(CardFinalScoring.Shaman_2, CardImmediate.Brick_1);
        public static Card BS22 = new Card(CardFinalScoring.Shaman_2, CardImmediate.Roll_Wood);

        public static Card BT11 = new Card(CardFinalScoring.ToolMaker_1, CardImmediate.Tool_4);
        public static Card BT12 = new Card(CardFinalScoring.ToolMaker_1, CardImmediate.Tool_3);
        public static Card BT21 = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Tool_2);
        public static Card BT22 = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Lottery);
        public static Card BT23 = new Card(CardFinalScoring.ToolMaker_2, CardImmediate.Lottery);

        private Card(CardFinalScoring cardFinalScoring, CardImmediate cardImmediate)
        {
            CardFinalScoring = cardFinalScoring;
            CardImmediate = cardImmediate;

            All.Add(this);
        }

        public CardImmediate CardImmediate;
        public CardFinalScoring CardFinalScoring;
    }
}
