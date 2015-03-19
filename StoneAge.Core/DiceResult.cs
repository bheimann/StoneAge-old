using System.Collections.Generic;
using System.Linq;
using StoneAge.Core.Exceptions;

namespace StoneAge.Core
{
    public class DiceResult
    {
        private bool _needsSorted = false;

        private List<int> _dice = new List<int>();

        public DiceResult(IEnumerable<int> dice)
        {
            foreach (var die in dice)
                Add(die);
            _dice.Sort();
        }

        private void Add(int dieValue)
        {
            if (dieValue > 6 || dieValue < 1)
            {
                throw new InvadidDieNumberException(dieValue);
            }
            _dice.Add(dieValue);
        }

        public IEnumerable<int> Summary()
        {
            return _dice;
        }

        public int this[int index]
        {
            get
            {
                return _dice[index];
            }
        }

        public int Sum()
        {
            return _dice.Sum();
        }
    }
}
