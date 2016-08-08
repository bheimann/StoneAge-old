using System.Collections.Generic;

namespace StoneAge.Core.Models.BuildingTiles
{
    public interface IBuildingTilePileCreator
    {
        IEnumerable<BuildingTile> Shuffle();
    }
}
