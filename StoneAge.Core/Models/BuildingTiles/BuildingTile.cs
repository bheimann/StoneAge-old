using System.Collections.Generic;

namespace StoneAge.Core.Models.BuildingTiles
{
    public class BuildingTile
    {
        // Should be 28 total
        public static List<BuildingTile> All { get; } = new List<BuildingTile>();

        public static BuildingTile WWB1 { get; } = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Brick);
        public static BuildingTile WWS1 { get; } = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Stone);
        public static BuildingTile WWG1 { get; } = new BuildingTile(Resource.Wood, Resource.Wood, Resource.Gold);
        public static BuildingTile BBS1 { get; } = new BuildingTile(Resource.Brick, Resource.Brick, Resource.Stone);
        public static BuildingTile BBG1 { get; } = new BuildingTile(Resource.Brick, Resource.Brick, Resource.Gold);
        public static BuildingTile SSG1 { get; } = new BuildingTile(Resource.Stone, Resource.Stone, Resource.Gold);
        public static BuildingTile WBB1 { get; } = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Brick);
        public static BuildingTile WSS1 { get; } = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Stone);
        public static BuildingTile BSS1 { get; } = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Stone);

        public static BuildingTile WBS1 { get; } = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Stone);
        public static BuildingTile WBS2 { get; } = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Stone);
        public static BuildingTile WSG1 { get; } = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Gold);
        public static BuildingTile WSG2 { get; } = new BuildingTile(Resource.Wood, Resource.Stone, Resource.Gold);
        public static BuildingTile WBG1 { get; } = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Gold);
        public static BuildingTile WBG2 { get; } = new BuildingTile(Resource.Wood, Resource.Brick, Resource.Gold);
        public static BuildingTile BSG1 { get; } = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Gold);
        public static BuildingTile BSG2 { get; } = new BuildingTile(Resource.Brick, Resource.Stone, Resource.Gold);

        public static BuildingTile C411 { get; } = new BuildingTile(4, 1);
        public static BuildingTile C421 { get; } = new BuildingTile(4, 2);
        public static BuildingTile C431 { get; } = new BuildingTile(4, 3);
        public static BuildingTile C441 { get; } = new BuildingTile(4, 4);

        public static BuildingTile C511 { get; } = new BuildingTile(5, 1);
        public static BuildingTile C521 { get; } = new BuildingTile(5, 2);
        public static BuildingTile C531 { get; } = new BuildingTile(5, 3);
        public static BuildingTile C541 { get; } = new BuildingTile(5, 4);

        public static BuildingTile C171 { get; } = new BuildingTile(1, 7, 1);
        public static BuildingTile C172 { get; } = new BuildingTile(1, 7, 1);
        public static BuildingTile C173 { get; } = new BuildingTile(1, 7, 1);

        public readonly Resource? Resource1;
        public readonly Resource? Resource2;
        public readonly Resource? Resource3;

        public int? NumberOfUniqueResources { get; set; }
        public int? MinQuantity { get; set; }
        public int? MaxQuantity { get; set; }

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
