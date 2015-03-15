using StoneAge.Core.Models.BoardSpaces;
using StoneAge.Core.Models.BuildingTiles;
using StoneAge.Core.Models.Cards;
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

        public readonly BuildingTileStack HutStack1;
        public readonly BuildingTileStack HutStack2;
        public readonly BuildingTileStack HutStack3;
        public readonly BuildingTileStack HutStack4;

        public IList<Space> Spaces;

        public GameBoard()
            : this(new StandardCardDeckCreator())
        {
        }

        public GameBoard(ICardDeckCreator cardDeckCreator)
        {
            WoodAvailable = TOTAL_WOOD;
            BrickAvailable = TOTAL_BRICK;
            StoneAvailable = TOTAL_STONE;
            GoldAvailable = TOTAL_GOLD;

            CardDeck = new Stack<Card>(cardDeckCreator.Shuffle());

            var shuffledBuildingHutTiles = new List<BuildingTile>(BuildingTile.All.Shuffle());

            HutStack1 = new BuildingTileStack(shuffledBuildingHutTiles.Skip(0).Take(7));
            HutStack2 = new BuildingTileStack(shuffledBuildingHutTiles.Skip(7).Take(7));
            HutStack3 = new BuildingTileStack(shuffledBuildingHutTiles.Skip(14).Take(7));
            HutStack4 = new BuildingTileStack(shuffledBuildingHutTiles.Skip(21).Take(7));

            Tool1or2Available = TOTAL_1_2_TOOLS;
            Tool3or4Available = TOTAL_3_4_TOOLS;

            AddDefaultSpaces();
        }

        private void AddDefaultSpaces()
        {
            Spaces = new List<Space>
            {
                new Space(BoardSpace.HuntingGrounds, new UnlimitedSpacesForPeople()),
                new Space(BoardSpace.Forest, new CanPlace1To7People()),
                new Space(BoardSpace.ClayPit, new CanPlace1To7People()),
                new Space(BoardSpace.Quarry, new CanPlace1To7People()),
                new Space(BoardSpace.River, new CanPlace1To7People()),
                new Space(BoardSpace.ToolMaker, new CanOnlyPlace1Person()),
                new Space(BoardSpace.Hut, new CanOnlyPlace2People()),
                new Space(BoardSpace.Field, new CanOnlyPlace1Person()),
                new Space(BoardSpace.CivilizationCardSlot1, new CanOnlyPlace1Person()),
                new Space(BoardSpace.CivilizationCardSlot2, new CanOnlyPlace1Person()),
                new Space(BoardSpace.CivilizationCardSlot3, new CanOnlyPlace1Person()),
                new Space(BoardSpace.CivilizationCardSlot4, new CanOnlyPlace1Person()),
                new Space(BoardSpace.BuildingTileSlot1, new CanOnlyPlace1Person()),
                new Space(BoardSpace.BuildingTileSlot2, new CanOnlyPlace1Person()),
                new Space(BoardSpace.BuildingTileSlot3, new CanOnlyPlace1Person()),
                new Space(BoardSpace.BuildingTileSlot4, new CanOnlyPlace1Person()),
            };
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
