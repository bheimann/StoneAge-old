using System.Collections.Generic;

namespace StoneAge.Core.Models.BuildingTiles
{
    public class StandardBuildingTilePileCreator : IBuildingTilePileCreator
    {
        public IEnumerable<BuildingTile> Shuffle()
        {
            return BuildingTile.All.Shuffle();
        }
    }
}
