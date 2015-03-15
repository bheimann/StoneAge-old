using StoneAge.Core.Models.Tools;
using System.Collections.Generic;

namespace StoneAge.Core.Models.Players
{
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
            : this(STARTING_PEOPLE_COUNT, STARTING_FOOD_COUNT,
            STARTING_RESOURCE_COUNT, Tool.None)
        {
        }

        private PlayerBoard(int startingPeopleCount, int startingFoodCount,
            int startingResourceCount, Tool startingTool)
        {
            TotalPeople = startingPeopleCount;
            PeopleToPlace = TotalPeople;

            Food = startingFoodCount;

            Resources.Add(Resource.Wood, startingResourceCount);
            Resources.Add(Resource.Brick, startingResourceCount);
            Resources.Add(Resource.Stone, startingResourceCount);
            Resources.Add(Resource.Gold, startingResourceCount);

            Tools[0] = startingTool;
            Tools[1] = startingTool;
            Tools[2] = startingTool;

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
}
