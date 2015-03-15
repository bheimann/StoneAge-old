namespace StoneAge.Core.Models.BoardSpaces
{
    public class CanPlace1To7People : ISpaceEvaluator
    {
        public bool RangeIsValid(int quantity)
        {
            return quantity >= 1 && quantity <= 7;
        }

        public bool HasEnoughFreeSpaces(int usedSpaces, int quantity)
        {
            if (usedSpaces + quantity <= 7)
                return true;

            return false;
        }
    }
}
