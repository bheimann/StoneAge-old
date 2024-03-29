﻿using StoneAge.Core.Exceptions;
using StoneAge.Core.Models.BoardSpaces;
using StoneAge.Core.Models.BuildingTiles;
using StoneAge.Core.Models.Cards;
using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core.Models
{
    public class StandardGameBoard
    {
        //53 food counters (worth 198)
        //  16 x 1 value (16)
        //  16 x 2 value (32)
        //  12 x 5 value (60)
        //  9 x 10 value (90)

        public const int TOTAL_WOOD = 20;
        public const int TOTAL_BRICK = 16;
        public const int TOTAL_STONE = 12;
        public const int TOTAL_GOLD = 10;

        public const int TOTAL_1_2_TOOLS = 12;
        public const int TOTAL_3_4_TOOLS = 6;

        public int WoodAvailable { get; set; }
        public int BrickAvailable { get; set; }
        public int StoneAvailable { get; set; }
        public int GoldAvailable { get; set; }

        public int Tool1or2Available { get; set; }
        public int Tool3or4Available { get; set; }

        public readonly Stack<Card> CardDeck;
        public Card CardSlot1 { get; set; }
        public Card CardSlot2 { get; set; }
        public Card CardSlot3 { get; set; }
        public Card CardSlot4 { get; set; }

        public readonly BuildingTileStack HutStack1;
        public readonly BuildingTileStack HutStack2;
        public readonly BuildingTileStack HutStack3;
        public readonly BuildingTileStack HutStack4;

        public IList<Space> Spaces { get; set; }

        public StandardGameBoard()
            : this(new StandardCardDeckCreator(), new StandardBuildingTilePileCreator())
        {
        }

        public StandardGameBoard(ICardDeckCreator cardDeckCreator,
            IBuildingTilePileCreator buildingTilePileCreator)
        {
            WoodAvailable = TOTAL_WOOD;
            BrickAvailable = TOTAL_BRICK;
            StoneAvailable = TOTAL_STONE;
            GoldAvailable = TOTAL_GOLD;

            CardDeck = new Stack<Card>(cardDeckCreator.Shuffle());

            // TODO: move this into IBuildingTilePileCreator implementations
            var shuffledBuildingHutTiles = new List<BuildingTile>(buildingTilePileCreator.Shuffle());

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
                new Space(BoardSpace.HuntingGrounds, new CanPlaceUnlimitedPeople()),
                new Space(BoardSpace.Forest, new CanPlace1To7People()),
                new Space(BoardSpace.ClayPit, new CanPlace1To7People()),
                new Space(BoardSpace.Quarry, new CanPlace1To7People()),
                new Space(BoardSpace.River, new CanPlace1To7People()),
                new Space(BoardSpace.ToolMaker, new CanOnlyPlace1Person()),
                new Space(BoardSpace.Hut, new MustPlaceExactly2People()),
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

        public Card GetCardFromSpace(BoardSpace cardSlot)
        {
            Card card;
            switch (cardSlot)
            {
                case BoardSpace.CivilizationCardSlot1:
                    card = CardSlot1;
                    break;
                case BoardSpace.CivilizationCardSlot2:
                    card = CardSlot2;
                    break;
                case BoardSpace.CivilizationCardSlot3:
                    card = CardSlot3;
                    break;
                case BoardSpace.CivilizationCardSlot4:
                    card = CardSlot4;
                    break;
                default:
                    throw new InvalidSpaceForCardsException(cardSlot);
            }
            return card;
        }
    }
}
