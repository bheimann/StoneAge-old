using System.Collections.Generic;
using System.Linq;
using StoneAge.Core.Exceptions;

namespace StoneAge.Core
{
    public class DiceResult
    {
        private bool _needsSorted = false;

        private List<int> _dice = new List<int>();

        public IEnumerable<int> Summary()
        {
            if (_needsSorted)
                _dice.Sort();
            _needsSorted = false;
            
            return _dice;
        }

        public int this[int index]
        {
            get
            {
                if (_needsSorted)
                    _dice.Sort();
                _needsSorted = false;

                return _dice[index];
            }
        }

        public void Add(int dieValue)
        {
            if (dieValue > 6 || dieValue < 1)
            {
                throw new InvadidDieNumberException(dieValue);
            }
            _needsSorted = true;
            _dice.Add(dieValue);
        }

        public int Sum()
        {
            return _dice.Sum();
        }
    }
}
