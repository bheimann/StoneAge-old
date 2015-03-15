namespace StoneAge.Core.Models.BoardSpaces
{
    public class CanOnlyPlace2People : ISpaceEvaluator
    {
        public bool RangeIsValid(int quantity)
        {
            return quantity == 2;
        }

        public bool HasEnoughFreeSpaces(int usedSpaces, int quantity)
        {
            if (usedSpaces == 2)
                return false;
            return true;
        }
    }
}
