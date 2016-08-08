namespace StoneAge.Core.Models.BoardSpaces
{
    public class CanPlaceUnlimitedPeople : ISpaceEvaluator
    {
        public bool RangeIsValid(int quantity)
        {
            return true;
        }

        public bool HasEnoughFreeSpaces(int usedSpaces, int quantity)
        {
            return true;
        }
    }
}
