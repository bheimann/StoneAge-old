using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core.Models
{
    public class GameBoard
    {
        public const int TOTAL_WOOD = 20;
        public const int TOTAL_BRICK = 16;
        public const int TOTAL_STONE = 12;
        public const int TOTAL_GOLD = 10;

        public const int TOTAL_1_2_TOOLS = 12;
        public const int TOTAL_3_4_TOOLS = 6;

        public int WoodAvailable;
        public int BrickAvailable;
        public int StoneAvailable;
        public int GoldAvailable;

        public int Tool1or2Available;
        public int Tool3or4Available;

        public readonly Stack<Card> CardDeck;
        public Card CardSlot1;
        public Card CardSlot2;
        public Card CardSlot3;
        public Card CardSlot4;

        public readonly HutStack HutStack1;
        public readonly HutStack HutStack2;
        public readonly HutStack HutStack3;
        public readonly HutStack HutStack4;

        public IList<Space> Spaces;

        public GameBoard()
        {
            WoodAvailable = TOTAL_WOOD;
            BrickAvailable = TOTAL_BRICK;
            StoneAvailable = TOTAL_STONE;
            GoldAvailable = TOTAL_GOLD;

            CardDeck = new Stack<Card>(Card.All.Shuffle());

            var shuffledBuildingHutTiles = new List<BuildingTile>(BuildingTile.All.Shuffle());

            HutStack1 = new HutStack(shuffledBuildingHutTiles.Skip(0).Take(7));
            HutStack2 = new HutStack(shuffledBuildingHutTiles.Skip(7).Take(7));
            HutStack3 = new HutStack(shuffledBuildingHutTiles.Skip(14).Take(7));
            HutStack4 = new HutStack(shuffledBuildingHutTiles.Skip(21).Take(7));

            Tool1or2Available = TOTAL_1_2_TOOLS;
            Tool3or4Available = TOTAL_3_4_TOOLS;

            AddDefaultSpaces();
        }

        private void AddDefaultSpaces()
        {
            Spaces = new List<Space>
            {
                new Space(BoardSpace.HuntingGrounds, maxQuantity: 40),
                //new Space(BoardSpace.Forest, maxQuantity: 7),
                //new Space(BoardSpace.ClayPit, maxQuantity: 7),
                //new Space(BoardSpace.Quarry, maxQuantity: 7),
                //new Space(BoardSpace.River, maxQuantity: 7),
                new Space(BoardSpace.ToolMaker, maxQuantity: 1),
                new Space(BoardSpace.Hut, 2, 2),
                new Space(BoardSpace.Field, maxQuantity: 1),
                new Space(BoardSpace.CivilizationCardSlot1, maxQuantity: 1),
                new Space(BoardSpace.CivilizationCardSlot2, maxQuantity: 1),
                new Space(BoardSpace.CivilizationCardSlot3, maxQuantity: 1),
                new Space(BoardSpace.CivilizationCardSlot4, maxQuantity: 1),
                new Space(BoardSpace.BuildingTileSlot1, maxQuantity: 1),
                new Space(BoardSpace.BuildingTileSlot2, maxQuantity: 1),
                new Space(BoardSpace.BuildingTileSlot3, maxQuantity: 1),
                new Space(BoardSpace.BuildingTileSlot4, maxQuantity: 1),
            };
        }

        //public void Next()
        //{
        //    //++_current;
        //    //if (_current >= Players.Count())
        //    //    _current = 0;

        //    //foreach (Space space in Spaces.Values)
        //    //{
        //    //    if (space.HeldBy == null)
        //    //    {
        //    //        space.HeldBy = space.ThinkingOf;
        //    //        space.ThinkingOf = null;
        //    //    }
        //    //}
        //}

        //public void TryToOccupySpace(BoardSpace Space)
        //{
        //    //if (Spaces[Space].HeldBy.HasValue)
        //    //    return;

        //    //if (Spaces[Space].ThinkingOf == null)
        //    //    Spaces[Space].ThinkingOf = Current.Color;
        //    //else
        //    //    Spaces[Space].ThinkingOf = null;
        //}

        //public PlayerColor? ColorOfSpace(BoardSpace Space)
        //{
        //    if (Spaces[Space].HeldBy.HasValue)
        //    {
        //        return Spaces[Space].HeldBy;
        //    }
        //    return Spaces[Space].ThinkingOf;
        //}
    }

    public class HutStack
    {
        private readonly Stack<BuildingTile> _stack;

        public bool HasTopFlipped { get; private set; }

        public HutStack(IEnumerable<BuildingTile> buildingTiles)
        {
            _stack = new Stack<BuildingTile>(buildingTiles);
        }

        public void FlipTopCard()
        {
            HasTopFlipped = true;
        }

        public BuildingTile Top()
        {
            if (HasTopFlipped)
                return _stack.Peek();
            // TODO: should this throw an exception?
            return null;
        }

        public BuildingTile TakeTop()
        {
            if (HasTopFlipped)
                return _stack.Pop();
            // TODO: should this throw an exception?
            return null;
        }

        public int Remaining
        {
            get
            {
                return _stack.Count();
            }
        }

        public bool IsEmpty
        {
            get
            {
                return !_stack.Any();
            }
        }
    }

    public class Space
    {
        public readonly BoardSpace BoardSpace;
        public readonly int MinQuantity;
        public readonly int MaxQuantity;

        private Dictionary<PlayerColor, int> _quantitiesPerColor = new Dictionary<PlayerColor, int>();

        public Space(BoardSpace boardSpace, int minQuantity = 1, int maxQuantity = 10)
        {
            BoardSpace = boardSpace;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            _quantitiesPerColor = new Dictionary<PlayerColor, int>();
        }
        
        public bool QuantityIsInvalidForSpace(int quantity)
        {
            if (quantity < MinQuantity || quantity > MaxQuantity)
                return true;

            if (BoardSpace == BoardSpace.Hut && quantity != 2)
                return true;

            return false;
        }

        public bool PlayerPreviouslyPlaced(Player player)
        {
            if (!_quantitiesPerColor.ContainsKey(player.Color))
                _quantitiesPerColor.Add(player.Color, 0);

            if (_quantitiesPerColor[player.Color] > 0)
                return true;

            return false;
        }

        public bool NotAvailable(int quantity)
        {
            if (_quantitiesPerColor.Values.Sum() + quantity > MaxQuantity)
                return true;

            return false;
        }

        public bool HasTooManyUniquePlayers()
        {
            // TODO: force number of unique player limit to space
            // TODO: is there a limit in hunting grounds?

            return false;
        }

        public void Place(Player player, int quantity)
        {
            if (!_quantitiesPerColor.ContainsKey(player.Color))
                _quantitiesPerColor.Add(player.Color, quantity);

            _quantitiesPerColor[player.Color] += quantity;
        }
    }

//53 food counters (worth 198)
//  16 x 1 value (16)
//  16 x 2 value (32)
//  12 x 5 value (60)
//  9 x 10 value (90)

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
        Wood = 3,
        Brick = 4,
        Stone = 5,
        Gold = 6,
    }

    public enum SpecialAction
    {
        Take2ResourcesCard,
    }
}
