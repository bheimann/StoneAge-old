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

        public readonly Stack<BuildingTile> HutStack1;
        public readonly Stack<BuildingTile> HutStack2;
        public readonly Stack<BuildingTile> HutStack3;
        public readonly Stack<BuildingTile> HutStack4;

        //public Dictionary<BoardSpace, Space> Spaces = new Dictionary<BoardSpace, Space>();

        public GameBoard()
        {
            WoodAvailable = TOTAL_WOOD;
            BrickAvailable = TOTAL_BRICK;
            StoneAvailable = TOTAL_STONE;
            GoldAvailable = TOTAL_GOLD;

            CardDeck = new Stack<Card>(Card.All.Shuffle());

            var shuffledBuildingHutTiles = new List<BuildingTile>(BuildingTile.All.Shuffle());

            HutStack1 = new Stack<BuildingTile>(shuffledBuildingHutTiles.Skip(0).Take(7));
            HutStack2 = new Stack<BuildingTile>(shuffledBuildingHutTiles.Skip(7).Take(7));
            HutStack3 = new Stack<BuildingTile>(shuffledBuildingHutTiles.Skip(14).Take(7));
            HutStack4 = new Stack<BuildingTile>(shuffledBuildingHutTiles.Skip(21).Take(7));

            Tool1or2Available = TOTAL_1_2_TOOLS;
            Tool3or4Available = TOTAL_3_4_TOOLS;

            //AddDefaultSpaces();
        }

        //private void AddDefaultSpaces()
        //{
        //    var boardSpaces = System.Enum.GetValues(typeof(BoardSpace)).Cast<BoardSpace>().Where(bs => bs != BoardSpace.HuntingGrounds);

        //    foreach (var boardSpace in boardSpaces)
        //    {
        //        Spaces.Add(boardSpace, new Space());
        //    }
        //}

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
    
    //public class Space
    //{
    //    public PlayerColor? HeldBy;
    //    public PlayerColor? ThinkingOf;
    //}

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
