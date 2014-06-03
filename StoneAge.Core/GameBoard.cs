using System.Collections.Generic;

namespace StoneAge.Core
{
    public class GameBoard
    {
        public readonly IDictionary<PlayerColor, int> Scores = new Dictionary<PlayerColor, int>();
        public readonly IDictionary<PlayerColor, int> FoodTrack = new Dictionary<PlayerColor, int>();

        public GameBoard()
            : this(PlayerColor.Blue, PlayerColor.Green, PlayerColor.Red, PlayerColor.Yellow)
        {
        }

        public GameBoard(params PlayerColor[] args)
        {
            foreach (PlayerColor color in args)
            {
                Scores.Add(color, STARTING_SCORE);
                FoodTrack.Add(color, STARTING_FOOD_TRACK);
            }
        }

        private const int STARTING_SCORE = 0;
        private const int STARTING_FOOD_TRACK = 0;
    }

    public class PlayerBoard
    {
        public readonly PlayerColor Color;
        public int Food;
        public readonly IDictionary<Resource, int> Resources = new Dictionary<Resource, int>();
        public int People;
        public readonly Tool[] Tools = new Tool[3]; 

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
        }

        private const int STARTING_PEOPLE_COUNT = 5;
        private const int STARTING_FOOD_COUNT = 12;
        private const int STARTING_RESOURCE_COUNT = 0;
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
