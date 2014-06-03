using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoneAge.Core
{
    public class DicePouch
    {
        private Random _random = new Random();

        public DiceResult Roll(int diceCount)
        {
            var dice = new DiceResult();

            for (int i = 0; i < diceCount; i++)
            {
                dice.Add(_random.Next(1, 7));
            }

            return dice;
        }
    }
}
