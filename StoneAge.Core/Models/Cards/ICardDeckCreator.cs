using System.Collections.Generic;

namespace StoneAge.Core.Models.Cards
{
    public interface ICardDeckCreator
    {
        IEnumerable<Card> Shuffle();
    }
}
