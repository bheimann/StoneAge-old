namespace StoneAge.Core.Models.BoardSpaces
{
    public class CanOnlyPlace1Person : ISpaceEvaluator
    {
        public bool RangeIsValid(int quantity)
        {
            return quantity == 1;
        }

        public bool HasEnoughFreeSpaces(int usedSpaces, int quantity)
        {
            if (usedSpaces > 0)
                return false;
            return true;
        }
    }
}
