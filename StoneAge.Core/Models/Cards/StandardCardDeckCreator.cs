using System.Collections.Generic;

namespace StoneAge.Core.Models.Cards
{
    public class StandardCardDeckCreator : ICardDeckCreator
    {
        public IEnumerable<Card> Shuffle()
        {
            return Card.All.Shuffle();
        }
    }
}
