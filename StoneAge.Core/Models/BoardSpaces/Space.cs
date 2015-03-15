using StoneAge.Core.Models.Players;
using System.Collections.Generic;
using System.Linq;

namespace StoneAge.Core.Models.BoardSpaces
{
    [System.Diagnostics.DebuggerDisplay("{BoardSpace} {_quantityEvaluator}")]
    public class Space
    {
        public readonly BoardSpace BoardSpace;
        public readonly int MinQuantity;
        public readonly int MaxQuantity;
        private ISpaceEvaluator _quantityEvaluator;

        private Dictionary<PlayerColor, int> _quantitiesPerColor = new Dictionary<PlayerColor, int>();

        public Space(BoardSpace boardSpace, int minQuantity = 1, int maxQuantity = 10)
        {
            BoardSpace = boardSpace;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            _quantitiesPerColor = new Dictionary<PlayerColor, int>();
        }

        public Space(BoardSpace boardSpace, ISpaceEvaluator quantityEvaluator)
        {
            BoardSpace = boardSpace;
            _quantityEvaluator = quantityEvaluator;
            _quantitiesPerColor = new Dictionary<PlayerColor, int>();
        }

        public bool QuantityIsInvalidForSpace(int quantity)
        {
            if (_quantityEvaluator != null)
            if (_quantityEvaluator.RangeIsValid(quantity))
                return false;
            else
                return true;

            if (quantity < MinQuantity || quantity > MaxQuantity)
                return true;

            if (BoardSpace == BoardSpace.Hut && quantity != 2)
                return true;

            return false;
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
            if (_quantityEvaluator != null)
                if (_quantityEvaluator.HasEnoughFreeSpaces(usedSpaces, quantity))
                    return false;
                else
                    return true;

            if (usedSpaces + quantity > MaxQuantity)
                return true;

            return false;
        }

        public bool HasTooManyUniquePlayers()
        {
            // TODO: force number of unique player limit to space
            // TODO: is there a limit in hunting grounds?

            return false;
        }

        public void Place(Player player, int quantity)
        {
            if (!_quantitiesPerColor.ContainsKey(player.Color))
                _quantitiesPerColor.Add(player.Color, quantity);

            _quantitiesPerColor[player.Color] += quantity;
        }
    }
}
