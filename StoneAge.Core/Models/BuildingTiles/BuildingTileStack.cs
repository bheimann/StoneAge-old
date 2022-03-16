using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core.Models.BuildingTiles
{
    public class BuildingTileStack
    {
        private readonly Stack<BuildingTile> _stack;

        public bool HasTopFlipped { get; private set; }

        public BuildingTileStack(IEnumerable<BuildingTile> buildingTiles)
        {
            _stack = new Stack<BuildingTile>(buildingTiles);
        }

        public void FlipTopTile()
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

        public int Remaining => _stack.Count;

        public bool IsEmpty => !_stack.Any();
    }
}
