using System;
using System.Collections.Generic;

namespace StoneAge.Core.Models
{
    public class DicePouch
    {
        private readonly Random _random = new Random();

        public DiceResult Roll(int diceCount)
        {
            var rolls = new List<int>();
            for (int i = 0; i < diceCount; i++)
            {
                rolls.Add(_random.Next(1, 7));
            }

            var dice = new DiceResult(rolls);
            return dice;
        }
    }
}
