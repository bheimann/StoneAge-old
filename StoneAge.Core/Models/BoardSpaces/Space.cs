using StoneAge.Core.Models.Players;
using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core.Models.BoardSpaces
{
    [System.Diagnostics.DebuggerDisplay("{BoardSpace} {_quantityEvaluator}")]
    public class Space
    {
        public readonly BoardSpace BoardSpace;
        private readonly ISpaceEvaluator _quantityEvaluator;

        private readonly Dictionary<PlayerColor, int> _quantitiesPerColor;

        public Space(BoardSpace boardSpace, ISpaceEvaluator quantityEvaluator)
        {
            BoardSpace = boardSpace;
            _quantityEvaluator = quantityEvaluator;
            _quantitiesPerColor = new Dictionary<PlayerColor, int>();
        }

        public bool QuantityIsInvalidForSpace(int quantity)
        {
            if (_quantityEvaluator.RangeIsValid(quantity))
                return false;

            return true;
        }

        public bool PlayerPreviouslyPlaced(Player player)
        {
            if (!_quantitiesPerColor.ContainsKey(player.Color))
                _quantitiesPerColor.Add(player.Color, 0);

            if (_quantitiesPerColor[player.Color] > 0)
                return true;

            return false;
        }

        public bool NotAvailable(int quantity)
        {
            var usedSpaces = _quantitiesPerColor.Values.Sum();
            if (_quantityEvaluator.HasEnoughFreeSpaces(usedSpaces, quantity))
                return false;

            return true;
        }

        public bool HasTooManyUniquePlayers()
        {
            // TODO: force number of unique player limit to space
            // TODO: is there a limit in hunting grounds?

            return false;
        }

        public void Place(Player player, int quantity)
        {
            _quantitiesPerColor[player.Color] += quantity;
        }

        public int QuantityPlaced(Player player)
        {
            return _quantitiesPerColor[player.Color];
        }

        public void ReturnToPlayer(Player player)
        {
            player.PlayerBoard.PeopleToPlace += QuantityPlaced(player);
            _quantitiesPerColor[player.Color] = 0;
        }

        public bool AllowsPartialPlacement
        {
            get
            {
                // TODO: should this allow for house rules?
                return BoardSpace == BoardSpace.HuntingGrounds;
            }
        }
    }
}
