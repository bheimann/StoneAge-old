using System.Collections.Generic;

namespace StoneAge.Core.Models.BuildingTiles
{
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
}
